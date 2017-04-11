using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Model;
using System.Data;
using Dapper;
using Model.DTO;

namespace Services
{
    public class DispatchService : IDispatchInterface
    {
        public bool InsertDispatch(InputDispatch model)
        {
            using (IDbConnection conn = SqlConn.OpenConnectionPS())
            {
                var sModel = conn.Query<OrdDispatch>("select * from OrdDispatch where state=0 and ConstId=@ConstId and RoleId=@RoleId", new { ConstId = model.ConstId, RoleId = model.RoleId }, commandType: CommandType.Text).ToList();
                int result = 0;

                //抢单成功
                if (sModel.Any())
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"InsertOrdByUser");

                    result = conn.Query<int>(strSql.ToString(), new { ConstId = model.ConstId, UserId = model.UserId, RoleId = model.RoleId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                //else
                //{
                //    StringBuilder strSql = new StringBuilder();
                //    strSql.Append(@"update OrdMembers set state=3,createTime=Getdate() where ConstId=@ConstId and UId=@UserId");

                //    result = conn.Execute(strSql.ToString()
                //        , new { ConstId = model.ConstId, UserId = model.UserId });
                //}

                return result > 0 ? true : false;
            }
        }

        public List<OrdDispatchDto> GetDispatch()
        {
            using (IDbConnection conn = SqlConn.OpenConnectionPS())
            {
                string sql = @"select od.*,ui.Phone,c.Name from OrdDispatch od left join Construct c
	                            on od.ConstId=c.Id left join
	                            ConstMemberRelation cmr on od.ConstId=cmr.ConstId
	                            left join UserInfo ui on cmr.UserId=ui.Id  where od.State=0 and ui.role=4 order by ConstId";
                var result = conn.Query<OrdDispatchDto>(sql, null, commandType: CommandType.Text).ToList();
                return result;
            }
        }

        public void DelDispatch(SendSmsDto model)
        {
            using (IDbConnection conn = SqlConn.OpenConnectionPS())
            {
                string sql = @"update OrdDispatch set state=2 where ConstId=@ConstId and RoleId=@RoleId";
                var result = conn.Execute(sql, new { ConstId = model.ConstId, RoleId = model.RoleId }, commandType: CommandType.Text);

            }
        }
    }
}
