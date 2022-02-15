using DST.Database.Model;
using SQLite.CodeFirst;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DST.PIMS.Framework
{
    public class LocalDBContext : DbContext
    {
        private static readonly Type[] types = typeof(SqliteEntity).Assembly.GetTypes();

        public LocalDBContext(string nameOrConnectionString)
           : base(nameOrConnectionString)
        {
            Configure();
        }

        public LocalDBContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;

            //// Sets the command timeout for all the commands
            //objectContext.CommandTimeout = int.MaxValue;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            InitTables(modelBuilder);
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<LocalDBContext>(modelBuilder);
            System.Data.Entity.Database.SetInitializer(sqliteConnectionInitializer);
        }
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void InitTables(DbModelBuilder modelBuilder)
        {
            var methodInfo = modelBuilder.GetType().GetMethod(nameof(modelBuilder.Entity)); // 获取entuty方法用以初始化表格
            var baseType = typeof(SqliteEntity);
            //var types = baseType.Assembly.GetTypes();
            foreach (var type in types)
            {
                if (baseType.IsAssignableFrom(type) && type != baseType)
                {
                    var genMethodInfo = methodInfo.MakeGenericMethod(type);
                    dynamic entityTypeConfiguration = genMethodInfo.Invoke(modelBuilder, null);
                    entityTypeConfiguration.ToTable(type.Name); // 建立表
                }
            }
        }
    }

}
