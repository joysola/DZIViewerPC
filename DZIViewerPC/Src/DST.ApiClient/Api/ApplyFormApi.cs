using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    /// <summary>
    /// 申请单相关接口
    /// </summary>
    class ApplyFormApi : BaseApi<ApplyFormApi>
    {
        #region 申请单
        /// <summary>
        /// 根据实验室编号（条码号、his识别号）获取申请单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology-case/getPathologyFormByLaboratoryCode")]
        [HttpGet]
        internal ApiResponse<ApplyFrmModel> GetPathInfobyCode([ParamName("laboratoryCode")] string code) => GetResult();

        /// <summary>
        /// 保存申请单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology-case/savePathologyForm")]
        [HttpPost]
        internal ApiResponse<List<AppFrmPrintCode>> SavePathInfo([PostContent] ApplyFrmModel applyInfo) => GetResult();

        /// <summary>
        /// 保存外送申请单
        /// </summary>
        /// <param name="applyInfo"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology-case/savePathologyFormByOutCall")]
        [HttpPost]
        internal ApiResponse<List<AppFrmPrintCode>> SavePathandSend([PostContent] ApplyFrmModel applyInfo) => GetResult();

        #endregion 申请单


        #region 病理申请单图片管理
        /// <summary>
        /// 病理id查询 申请单图片列表
        /// </summary>
        /// <param name="pathologyId">病理号</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology-image/listByPathologyId")]
        [HttpGet]
        internal ApiResponse<List<PathologyImage>> ListByPathologyId(string pathologyId) => GetResult();

        /// <summary>
        /// 删除申请单图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology-image/removeById")]
        [HttpGet]
        internal ApiResponse<object> RemoveById(string id) => GetResult();

        /// <summary>
        /// 申请单保存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/pathology-image/savePathologyImage")]
        [HttpPost]
        internal ApiResponse<object> SavePathologyImage([PostContent] PathologyImage pathologyImage) => GetResult();
        #endregion 病理申请单图片管理
    }
}
