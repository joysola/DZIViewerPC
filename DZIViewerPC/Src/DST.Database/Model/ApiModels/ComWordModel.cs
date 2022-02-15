using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ComWordModel : BaseModel
    {


        /// <summary>
        /// 场景code
        /// </summary>
        [JsonProperty("sceneCode")]
        public string SceneCode { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("typeId")]
        public string TypeID { get; set; }
        /// <summary>
        /// 常用词
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }


    public class ComWordType : BaseModel
    {

        /// <summary>
        /// 场景code
        /// </summary>
        [JsonProperty("sceneCode")]
        public string SceneCode { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
