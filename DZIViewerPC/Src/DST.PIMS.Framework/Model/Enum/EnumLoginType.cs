using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Model.Enum
{
    public enum EnumLoginType
    {
        /// <summary>
        /// 正常登录
        /// </summary>
        [Description("正常登录")]
        NormalLogin = 5,

        /// <summary>
        /// 有文件正在上传
        /// </summary>
        [Description("有文件正在上传")]
        Uploading = 10,

        /// <summary>
        /// 关闭计算机
        /// </summary>
        [Description("关闭计算机")]
        ShutDown = 15,
    }
}
