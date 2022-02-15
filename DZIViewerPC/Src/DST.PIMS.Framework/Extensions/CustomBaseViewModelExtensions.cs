using DST.Controls.Base;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Extensions
{
    public static class CustomBaseViewModelExtensions
    {
        /// <summary>
        /// 获取CustomBaseViewModel所有的virtual protected 方法
        /// </summary>
        private static Dictionary<string, MethodInfo> ButtonMethods { get; set; } = new Lazy<Dictionary<string, MethodInfo>>(() =>
        {
            var result = new Dictionary<string, MethodInfo>();
            var customBaseViewModelType = Type.GetType("DST.PIMS.Client.ViewModel.CustomBaseViewModel,DST.PIMS.Client.ViewModel"); // 获取CustomBaseViewModel
            var methods = customBaseViewModelType?.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)?.Where(x => x.IsVirtual && x.IsFamily); // protected 和 virtual 方法
            foreach (var mm in methods)
            {
                result.Add(mm.Name.ToLower(), mm);
            }
            return result;
        }).Value;
        /// <summary>
        /// MethodBase.Invoke方法调用的表达式树
        /// </summary>
        public static Action<MethodInfo, BaseViewModel> MethodInvokeAction { get; } = new Lazy<Action<MethodInfo, BaseViewModel>>(() =>
        {
            var param_methodInfo = Expression.Parameter(typeof(MethodInfo), "methodInfo");
            var param_viewModel = Expression.Parameter(typeof(BaseViewModel), "viewModel");
            var const_objectArrty = Expression.Constant(null, typeof(object[]));
            var methodInvoke = typeof(MethodBase).GetMethod(nameof(MethodBase.Invoke), new Type[] { typeof(object), typeof(object[]) });
            var method_exp = Expression.Call(param_methodInfo, methodInvoke, param_viewModel, const_objectArrty);
            var action = Expression.Lambda<Action<MethodInfo, BaseViewModel>>(method_exp, param_methodInfo, param_viewModel).Compile();
            return action;
        }).Value;
        /// <summary>
        /// 调用custombaseviewmodel对应的按钮方法
        /// </summary>
        /// <param name="viewModel">custombaseview的对象</param>
        /// <param name="enumToolButton"></param>
        public static void InvokeToolButtonMethod(this BaseViewModel viewModel, EnumToolButtonType enumToolButton)
        {
            //var name = Enum.GetName(typeof(EnumToolButtonType), enumToolButton); // 获取枚举名称
            var name = ExtendAppContext.Current.AllButtons?.FirstOrDefault(x => x.ToolBtnType == enumToolButton)?.EnumName;
            if (!string.IsNullOrEmpty(name) && ButtonMethods.TryGetValue(name.ToLower(), out MethodInfo methodInfo))
            {
                //methodInfo?.Invoke(viewModel, null);
                MethodInvokeAction(methodInfo, viewModel);
            }
        }
    }
}
