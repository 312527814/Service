using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Data;
using Dapper;
using Model;

namespace Services
{
    public class TestService : ITestInterface
    {
        public bool InsertTest(Test model)
        {
            using (IDbConnection conn = SqlConn.OpenTestConnection())
            {
                var sModel = conn.Query<TestDto>("select * from Test where state=0").ToList();
                int result = 0;
                if (sModel.Any())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update Test set state=1,createTime=Getdate() where state=0");

                    result = conn.Execute(strSql.ToString(), null);
                }
                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"INSERT INTO Test(Name,State,CreateTime)
                                    VALUES(@Name, @State, @CreateTime)");

                    result = conn.Execute(strSql.ToString(), new { Name = model.ProcessId + "+" + model.Sort, State = 2, CreateTime = DateTime.Now });
                }




                return result > 0 ? true : false;
            }
        }
    }
}
