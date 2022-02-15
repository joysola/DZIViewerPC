using DST.ApiClient.Api;
using DST.Database.Model;
using HttpClientExtension.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Service
{
    public class ComWordService : BaseService<ComWordService>
    {
        #region 常用词
        /// <summary>
        /// 查询常用词列表
        /// </summary>
        /// <param name="content">常用词</param>
        /// <param name="typeID">类型id</param>
        /// <returns></returns>
        public List<ComWordModel> GetComWordList(string content, string typeID, string sceneCode)
        {
            var result = new List<ComWordModel>();
            content = content ?? string.Empty;
            typeID = typeID ?? string.Empty;
            var response = ComWordApi.Client.GetComWordList(content, typeID, sceneCode);
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }
        /// <summary>
        /// 常用词删除
        /// </summary>
        /// <param name="comWord">常用词实体</param>
        /// <returns></returns>
        public bool DeleteComWord(ComWordModel comWord)
        {
            if (!string.IsNullOrEmpty(comWord?.ID))
            {
                var response = ComWordApi.Client.DeleteComWord(comWord.ID);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 常用词新增或更新
        /// </summary>
        /// <param name="comWord">常用词实体</param>
        /// <returns></returns>
        public bool SaveComWord(ComWordModel comWord)
        {
            if (comWord != null)
            {
                var response = ComWordApi.Client.SaveComWord(comWord);
                return response.Success;
            }
            return false;
        }
        #endregion 常用词

        #region 常用词类型
        /// <summary>
        /// 查询常用词类型列表
        /// </summary>
        /// <param name="sceneCode">场景（类型）id</param>
        /// <returns></returns>
        public List<ComWordType> GetComWordTypeList(string sceneCode)
        {
            var result = new List<ComWordType>();
            var response = ComWordApi.Client.GetComWordTypeList(sceneCode);
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }
        /// <summary>
        /// 常用词类型新增或删除
        /// </summary>
        /// <param name="comWordType">常用词类型实体</param>
        /// <returns></returns>
        public bool SaveComWordType(ComWordType comWordType)
        {
            if (comWordType != null)
            {
                var response = ComWordApi.Client.SaveComWordType(comWordType);
                return response.Success;
            }
            return false;
        }
        #endregion 常用词类型

    }
}
