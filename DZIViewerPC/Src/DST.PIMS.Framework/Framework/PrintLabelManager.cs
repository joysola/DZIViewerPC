using DST.Database.Model;
using DST.PIMS.Framework.Attributes;
using DST.PIMS.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DST.Common.Helper;
using System.Collections;

namespace DST.PIMS.Framework
{
    /// <summary>
    /// 打印管理对应的接口
    /// </summary>
    interface IPrintLabelManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sectionName"></param>
        void InitPrint(string sectionName);

        #region 打印
        void Print(List<EmbedPrintCode> barcodeModelList);
        void Print(List<PhysDistReceiptBarcodeModel> barcodeModelList);
        void Print(List<AppFrmPrintCode> appFrmPrintCodes);
        void Print(List<SlicePrintCode> slicePrintCodes);
        #endregion 打印
    }
    /// <summary>
    /// 打印机主处理类
    /// </summary>
    public class PrintLabelManager
    {
        public static PrintLabelManager Singleton { get; } = new PrintLabelManager();
        private PrintLabelManager() { }
        /// <summary>
        /// 打印管理实体信息
        /// </summary>
        class PrintManagerInfo
        {
            /// <summary>
            /// 实例
            /// </summary>
            public IPrintLabelManager ManagerInstance { get; set; }
            /// <summary>
            /// 实例类型
            /// </summary>
            public Type ManagerType { get; set; }
            /// <summary>
            /// 打印方法参数，对应委托字典
            /// </summary>
            public Dictionary<Type, dynamic> ActionDict { get; set; } = new Dictionary<Type, dynamic>();
        }
        /// <summary>
        /// 设定实体
        /// </summary>
        private PrintSetting Setting { get; set; }

        /// <summary>
        /// 打印委托集合
        /// </summary>
        private Dictionary<string, PrintManagerInfo> PrintManagerInfoDict { get; } = new Lazy<Dictionary<string, PrintManagerInfo>>(() =>
        {
            var result = new Dictionary<string, PrintManagerInfo>();

            var printTypes = Assembly.Load("DST.Database")?.GetTypes()?.Where(x => typeof(ILabelPrint).IsAssignableFrom(x) && !x.IsInterface)?.Select(x => x.Name)?.ToList();
            var printTypeList = printTypes?.Count > 0 ? printTypes : new List<string>(); // 打印类型名称集合

            var types = Assembly.GetExecutingAssembly()?.GetTypes();
            printTypeList.ForEach(pt =>
            {
                var pType = types.FirstOrDefault(x => x.GetCustomAttribute<PrintManagerAttribute>()?.PrintType == pt);
                if (pType != null)
                {
                    var instance = (IPrintLabelManager)Activator.CreateInstance(pType); // 创建对应管理类实体
                    result.Add(pt, new PrintManagerInfo { ManagerInstance = instance, ManagerType = pType });
                }
            });

            var methodList = typeof(IPrintLabelManager).GetMethods().ToList(); // 打印类需要实现统一接口，默认名称Print

            methodList.ForEach(m =>
            {
                var paramTypes = m.GetParameters().Select(x => x.ParameterType).ToArray(); // 接口方法的参数
                foreach (var item in result)
                {
                    if (paramTypes?.Length == 1 && typeof(IList).IsAssignableFrom(paramTypes[0]))// 默认只有一个参数,且参数是IList集合类型
                    {
                        var printMethod = item.Value.ManagerType.GetMethod(m.Name, paramTypes); // 根据方法名和方法参数 获取 对应方法信息
                        var actionType = typeof(Action<>).MakeGenericType(paramTypes[0]); // 构造泛型方法
                        dynamic action = printMethod.CreateDelegate(actionType, item.Value.ManagerInstance); // 创建委托用于之后调用

                        if (result.TryGetValue(item.Key, out PrintManagerInfo printManagerInfo))
                        {
                            printManagerInfo.ActionDict.Add(paramTypes[0], action);
                        }
                    }
                }
            });
            return result;

        }).Value;

        /// <summary>
        /// 打印
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="printCodes">打印实体集合</param>
        public void Print<T>(List<T> printCodes)
        {
            var paramType = typeof(List<T>);
            if (Setting.IsMix) // 混合打
            {
                foreach (var keyvalue in PrintManagerInfoDict)
                {
                    if (keyvalue.Value.ActionDict.TryGetValue(paramType, out dynamic action))
                    {
                        action(printCodes);
                    }
                }
            }
            else // 单打
            {
                if (!string.IsNullOrEmpty(Setting?.PrintType) && PrintManagerInfoDict.TryGetValue(Setting?.PrintType, out PrintManagerInfo printManagerInfo))
                {
                    if (printManagerInfo.ActionDict.TryGetValue(paramType, out dynamic action))
                    {
                        action(printCodes);
                    }
                }
            }
        }


        /// <summary>
        /// 初始化打印
        /// </summary>
        /// <param name="sectionName">节名称</param>
        public void InitPrintManager(string sectionName)
        {
            var setting = PrintSetHelper.GetPrintSetting(sectionName);
            if (setting != null)
            {
                Setting = setting;
                if (setting.IsMix) // 混合都要初始化
                {
                    foreach (var keyvalue in PrintManagerInfoDict)
                    {
                        keyvalue.Value.ManagerInstance?.InitPrint(sectionName);
                    }
                }
                else // 单打初始化
                {
                    if (!string.IsNullOrEmpty(Setting?.PrintType) && PrintManagerInfoDict.TryGetValue(Setting?.PrintType, out PrintManagerInfo printManagerInfo))
                    {
                        printManagerInfo.ManagerInstance?.InitPrint(sectionName); // 初始化
                    }
                }
            }
        }


    }
}
