using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class QueryResetPW
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }

    public class QueryLockUser
    {
        /// <summary>
        /// 是否需要锁住
        /// </summary>
        [JsonProperty("isLock")]
        public bool IsLock { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty("userId")]
        public string UserID { get; set; }
    }
}
