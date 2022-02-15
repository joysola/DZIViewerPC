using DST.Database.Model;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace DST.PIMS.Framework
{
    public class TreatRepertManager
    {
        public static TreatRepertManager Instance { get; } = new TreatRepertManager();

        private TreatRepertManager()
        {
        }

        public DataTable TreatRepertListToDataTable(List<TreatRepertModel> data)
        {
            DataTable dt = null;
            if (data == null || data.Count == 0)
            {
                return dt;
            }

            try
            {
                dt = new DataTable();
                dt.Columns.Add("医院", typeof(string));
                dt.Columns.Add("姓名", typeof(string));
                dt.Columns.Add("年龄", typeof(string));
                dt.Columns.Add("性别", typeof(string));
                dt.Columns.Add("实验编码", typeof(string));
                dt.Columns.Add("检查项目", typeof(string));
                dt.Columns.Add("核酸检测", typeof(string));
                dt.Columns.Add("备注", typeof(string));

                data.ForEach(x =>
                {
                    dt.Rows.Add(new object[] { x.hospitalName, x.patientName, x.age, x.sex, x.laboratoryCode, x.productName, "", x.remark });
                });
            }
            catch(Exception ex)
            {
                Logger.Error("送检列表转换DataTable失败：" + ex.Message);
            }

            return dt;
        }
    }
}
