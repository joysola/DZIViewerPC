namespace DST.Database.Model
{
    /// <summary>
    /// 送检医生条目
    /// </summary>
    public class SubmitDoctorModel
    {
        /// <summary>
        /// 实际姓名
        /// </summary>
        public string realName { get; set; }

        /// <summary>
        /// 医生id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string username { get; set; }
    }
}