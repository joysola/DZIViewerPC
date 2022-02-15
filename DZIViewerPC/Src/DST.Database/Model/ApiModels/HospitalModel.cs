using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.ObjectModel;

namespace DST.Database.Model
{
    /// <summary>
    /// 共建医院信息
    /// </summary>
    public class HospitalModel
    {
        public string address { get; set; }
        public string areaAllName { get; set; }
        public string areaId { get; set; }
        public string cancersCustomerId { get; set; }
        public string code { get; set; }
        public string contacts { get; set; }
        public string cooperationMode { get; set; }
        public string createDept { get; set; }
        public string createUser { get; set; }
        //public Nullable<DateTime> firstOrderDate { get; set; }
        public string firstSpell { get; set; }
        public string grade { get; set; }
        public string id { get; set; }
        public Nullable<int> isDeleted { get; set; }
        public string name { get; set; }
        public string officePhone { get; set; }
        public string paymentDays { get; set; }
        public string reportConfig { get; set; }
        public string salesUserId { get; set; }
        public string salesUserName { get; set; }
        public string sampleType { get; set; }
        //public Nullable<DateTime> signDate { get; set; }
        public Nullable<int> sort { get; set; }
        public Nullable<int> status { get; set; }
        public string telphone { get; set; }
        public string type { get; set; }
        //public Nullable<DateTime> updateTime { get; set; }
        public string updateUser { get; set; }
    }

    public class HospitalReturnModel : ObservableObject
    {
        public int? pages { get; set; }
        public int? total { get; set; }
        public int? size { get; set; }
        public int? current { get; set; }

        [Notification]
        public ObservableCollection<HospitalModel> records { get; set; }
    }
}