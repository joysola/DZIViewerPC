using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using DST.Database.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Nico.Common;

namespace DST.ApiClient.Api
{
    class ComWordApi : BaseApi<ComWordApi>
    {
        #region 常用词
        /// <summary>
        /// 查询常用词列表
        /// </summary>
        /// <param name="content">常用词</param>
        /// <param name="typeId">类型id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/common-word/listWordBySceneCode")]
        [HttpGet]
        internal ApiResponse<List<ComWordModel>> GetComWordList(string content, string typeId, string sceneCode) => GetResult();

        /// <summary>
        /// 常用词删除
        /// </summary>
        /// <param name="id">常用词id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/common-word/removeWord")]
        [HttpPost]
        internal ApiResponse<object> DeleteComWord(string id) => GetResult();

        /// <summary>
        /// 常用词新增或更新
        /// </summary>
        /// <param name="comWord">常用词实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/common-word/saveWord")]
        [HttpPost]
        internal ApiResponse<object> SaveComWord([PostContent] ComWordModel comWord) => GetResult();
        #endregion 常用词

        #region 常用词类型
        /// <summary>
        /// 查询常用词类型列表
        /// </summary>
        /// <param name="sceneCode">场景（类型）id</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/common-word-type/listWordTypeBySceneCode")]
        [HttpGet]
        internal ApiResponse<List<ComWordType>> GetComWordTypeList(string sceneCode) => GetResult();
        /// <summary>
        /// 常用词类型新增或删除
        /// </summary>
        /// <param name="comWordType">常用词类型实体</param>
        /// <returns></returns>
        [Url("dst-pathology/pathology/common-word-type/saveWordType")]
        [HttpPost]
        internal ApiResponse<object> SaveComWordType([PostContent] ComWordType comWordType) => GetResult();
        #endregion 常用词类型
    }
}
