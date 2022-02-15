using DST.Database.Model;
using Snowflake.Core;
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework
{
    public class LocalDB
    {
        internal static LocalDB Instance { get; } = new LocalDB();

        //private static readonly LocalDBContext _context = new LocalDBContext("localDB2");
        internal static LocalDBContext Context => new LocalDBContext("localDB2");
        internal static IdWorker SnowflakeId { get; } = new IdWorker(1, 1);

        //internal static SQLiteAsyncConnection Connection => new SQLiteAsyncConnection("C:\\DST\\LocalDB2.db");
        //static LocalDB()
        //{
        //var conn = ConfigurationManager.ConnectionStrings["localDB"].ConnectionString;
        //var sqliteconn = new SQLiteConnection(conn);
        //sqliteconn.Open();
        //}

        //internal void Getxxx()
        //{
        //    var xx = _context.Set<Img_Anno>().SingleOrDefault(x => x.Id == "abc");
        //    xx.Update_User = "hhh";
        //    _context.SaveChanges();

        //    if (xx != null)
        //    {
        //        _context.Set<Img_Anno>().Remove(xx);
        //    }
        //    _context.Set<Img_Anno>().Add(new Img_Anno { Id = "abc", Sample_Id = "abcd", Create_Time = DateTime.Now });

        //    _context.SaveChanges();
        //}
    }
}
