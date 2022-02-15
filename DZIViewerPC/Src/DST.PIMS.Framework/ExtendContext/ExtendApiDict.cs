using DST.Common.Extensions;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using MVVMExtension;
using System.Collections.Generic;
using System.Linq;

namespace DST.PIMS.Framework.ExtendContext
{
    /// <summary>
    /// 字典
    /// </summary>
    public class ExtendApiDict
    {
        public static ExtendApiDict Instance { get; } = new ExtendApiDict();

        private ExtendApiDict()
        {
        }

        /// <summary>
        /// 检查项目状态状态字典
        /// </summary>
        public List<DictItem> CheckProjectStatusDict { get; set; }

        /// <summary>
        /// 导出状态
        /// </summary>
        public List<DictItem> DownFlagDict { get; set; }

        /// <summary>
        /// 性别字典
        /// </summary>
        public List<DictItem> SexDict { get; set; }

        /// <summary>
        /// 检查项目
        /// </summary>
        public List<ProductModel> ProductDict { get; set; }

        /// <summary>
        /// 送检医生字典
        /// </summary>
        public List<SubmitDoctorModel> SubmitDoctorDict { get; set; }

        /// <summary>
        /// 实验室状态字典
        /// </summary>
        public List<DictItem> ExperimentStatusDict { get; set; }

        /// <summary>
        /// 获取活检字典（0 无活检、1 有活检）
        /// </summary>
        public List<DictItem> BiopsyFlagDict { get; set; }
        /// <summary>
        /// 获取HPV结果字典（0 阴性、1 阳性）
        /// </summary>
        public List<DictItem> HPVResultDict { get; set; }
        /// <summary>
        /// 获取腺上皮细胞分析结果字典（-1 无、0 未见腺上皮内病变及恶性病变、1 非典型腺细胞（无指定）、2 原位腺癌 、3 非典型腺细胞（倾向瘤变）、4 腺癌）
        /// </summary>
        public List<DictItem> GlandularEpithelialCellResultDict { get; set; }
        /// <summary>
        /// 获取样本TCT诊断结果字典（0 未见上皮内病变及恶性病变（NILM)、1 非典型鳞状上皮细胞，无明确诊断意义（ASC-US)、2 非典型鳞状上皮细胞，不除外高度病变（ASC-H)、3 低度鳞状上皮内病变（LSIL)、4 高度鳞状上皮内病变（H-SIL)、5 鳞状细胞癌）
        /// </summary>
        public List<DictItem> SampleTctResultDict { get; set; }
        /// <summary>
        /// 获取样本标记状态字典（0 待发送、1 已发送、2 待标记、3 已完成）
        /// </summary>
        public List<DictItem> SampleSignStatusDict { get; set; }
        /// <summary>
        /// 获取标记结果字典（1_1 ASC-US、1_2 ASC-H、1_3 LSIL、1_4 HSIL、1_5 gandular、1_6 glandular-adace、1_7 atrophy、1_8 repair、1_9 metaplastic）
        /// </summary>
        public List<DictItem> SignResultDict { get; set; }

        /// <summary>
        /// 获取物流快递状态：已发货、已收货、以确认
        /// </summary>
        public List<DictItem> ExpressStatusDict { get; set; }
        /// <summary>
        /// 活动类型
        /// </summary>
        public List<DictItem> ActivityTypeDict { get; set; }
        /// <summary>
        /// 附言字典
        /// </summary>
        public List<DictItem> PostscriptDict { get; set; }
        /// <summary>
        /// 包埋盒打印状态
        /// </summary>
        public List<DictItem> EmbedPrintStatusDict { get; set; }
        /// <summary>
        /// 样本取材延迟原因
        /// </summary>
        public List<DictItem> SamplTissDelayDict { get; set; }
        /// <summary>
        /// 登记样本状态字典 0 未处理  1 已处理
        /// </summary>
        public List<DictItem> RegisterSampleStatusDict { get; set; }
        /// <summary>
        /// 取材状态字典 0 未完成  1 已完成
        /// </summary>
        public List<DictItem> DrawMaterStatusDict { get; set; }
        /// <summary>
        /// 医嘱状态 0 未执行 1 已执行
        /// </summary>
        public List<DictItem> AdviceStatusDict { get; set; }
        /// <summary>
        /// 技术医嘱类型 1  重切 2 深切 3 薄切 4 多切 5 连切 6 补切 7 重新扫描 8 重新制片
        /// </summary>
        public List<DictItem> TechAdviceDict { get; set; }
        /// <summary>
        /// 特检医嘱类型 99 免疫组化
        /// </summary>
        public List<DictItem> DoctAdviceDict { get; set; }
        private List<DictItem> _adviceDict = new List<DictItem>();
        /// <summary>
        /// 医嘱类型 
        /// </summary>
        public List<DictItem> AdviceDict
        {
            get
            {
                if (_adviceDict?.Count == 0 && DoctAdviceDict?.Count > 0 && TechAdviceDict?.Count > 0)
                {
                    _adviceDict.AddRange(DoctAdviceDict);
                    _adviceDict.AddRange(TechAdviceDict);
                }
                return _adviceDict;
            }
        }
        /// <summary>
        /// 试剂类型
        /// </summary>
        public List<DictItem> ProdReagentDict { get; set; }
        /// <summary>
        /// 胃镜字典
        /// </summary>
        public List<DictItem> GastroscopyTissueDict { get; set; }
        /// <summary>
        /// 重新取样字典
        /// </summary>
        public List<DictItem> ReSampStatusDict { get; set; }
        /// <summary>
        /// 重新取样原因字典
        /// </summary>
        public List<DictItem> ReSampReasonDict { get; set; }
        /// <summary>
        /// 重新取样撤销原因字典
        /// </summary>
        public List<DictItem> ReSampWithdrawReasonDict { get; set; }

        /// <summary>
        /// 接收状态字典：0=未接收，1=已接收
        /// </summary>
        public List<DictItem> ReceiveStatus { get; set; }

        /// <summary>
        /// 扫描状态：0=待扫描，1=已扫描
        /// </summary>
        public List<DictItem> ScanStatus { get; set; }
        /// <summary>
        /// 海世嘉打印类型
        /// </summary>
        public List<DictItem> HsjPrintType { get; set; }

        /// <summary>
        /// 实验状态 0 = 待实验，1 =已实验
        /// </summary>
        public List<DictItem> TrialStatusList { get; set; }

        /// <summary>
        /// hpv状态 0=未导入，1=已导入
        /// </summary>
        public List<DictItem> HpvStatusList { get; set; }

        /// <summary>
        /// PCR 检测结果
        /// </summary>
        public List<DictItem> LargeResultList { get; set; }

        /// <summary>
        /// 报告确认状态  0：未确认 1：已确认
        /// </summary>
        public List<DictItem> ReportStatusList { get; set; }

        /// <summary>
        /// 病理类型列表
        /// </summary>
        public List<DictItem> PathologyTypeList { get; set; }

        /// <summary>
        /// 根据项目ID和 项目类型ID 获取项目类型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        public ProductType GetProductTypeByProductTypeID(string productId, string productType)
        {
            ProductModel model = this.ProductDict.FirstOrDefault(x => x.id.Equals(productId));
            if (null != model && model.productTypeList != null)
            {
                return model.productTypeList.FirstOrDefault(x => x.id.Equals(productType));
            }

            return null;
        }

    }
}