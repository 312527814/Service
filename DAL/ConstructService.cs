using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Model;
using Interfaces;

namespace Services
{
    public class ConstructService : IConstructInterface
    {
        public int AddSMSLog(SMSEvalLog model)
        {
            using (IDbConnection conn = SqlConn.OpenConnectionPS())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"INSERT INTO SMSEvalLog(ConstId,Phone,ConstName,Phase,Role,CreateTime)
                                    VALUES(@ConstId, @Phone, @ConstName, @Phase,@Role, @CreateTime)");

                return conn.Execute(strSql.ToString(), new { ConstId = model.ConstId, Phone = model.Phone, ConstName = model.ConstName, Phase = model.Phase, Role = model.Role, CreateTime = model.CreateTime }
                );
            }
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <returns></returns>
        public List<SMSEvalLogDTO> GetConstruct()
        {
            List<SMSEvalLogDTO> list = new List<SMSEvalLogDTO>();

            //var p = new DynamicParameters();
            //p.Add("@nav_type", nav_type);

            using (IDbConnection conn = SqlConn.OpenConnectionPS())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("exec Eval_Remind");

                list = conn.Query<SMSEvalLogDTO>(strSql.ToString(), null).ToList();
            }

            return list;
        }

    }
}
