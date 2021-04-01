using System;
using SqlSugar;
using DB;

namespace Service
{
    public class DBServer:ServiceBase
    {
        public static string CurrentProjectPath 
        {
            get
            {
                return Environment.CurrentDirectory.Replace(@"\bin\Debug","");
            }
        }

        private SqlSugarClient db;
        private DBServer()
        {
            var connectionString = $@"DataSource ={CurrentProjectPath}\DataBase\test.db";
            
            Debug.Log(connectionString);
            db = new SqlSugarClient(new ConnectionConfig(){
                ConnectionString = connectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true
            });

            db.Aop.OnLogExecuting=(sql,pars)=>{
                Debug.Log(sql);
            };

            // db.DbMaintenance.CreateDatabase();
            // db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(DB.User));
        }

        public void Init()
        {
            var ls = db.Queryable<DB.User>().ToList();
            Debug.Log(ls.Count);
        }
    }
}
