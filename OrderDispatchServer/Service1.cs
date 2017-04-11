using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Services;
using Model.DTO;
using Comm;

namespace OrderDispatchServer
{
    /// <summary>
    /// 抢单一个小时内没人抢 人工处理提醒
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();//3600000  

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //执行频率  毫秒为单位
            //string timeNumber = System.Configuration.ConfigurationSettings.AppSettings["TimeNumber"].ToString();
            this.timer.Interval = 3600000;   //3600000   每小时执行一次
            //定时启动
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            timer.Start();
        }

        protected override void OnStop()
        {

        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IDispatchInterface bll = new DispatchService();
            SmsMessage c = new SmsMessage();
            List<OrdDispatchDto> list = bll.GetDispatch();

            //var ls = t.GroupBy(a => new { a.Sort }).Select(g => new TempList() { Sort = g.Key.Sort, Days = g.Max(item => item.Days) }).ToList();
            List<GroupDispatchDto> newlist = list.GroupBy(p => new { p.ConstId, p.Phone }).Select(g => new GroupDispatchDto() { ConstId = g.Key.ConstId, Phone = g.Key.Phone }).ToList();
            List<SendSmsDto> sendList = new List<SendSmsDto>();
            foreach (var item in newlist)
            {
                var temp = list.Where(p => p.ConstId == item.ConstId).ToList();
                if (temp[0].CreateTime.AddHours(24) < DateTime.Now)
                {
                    SendSmsDto senddto = new SendSmsDto();
                    senddto.ConstId = item.ConstId;
                    senddto.Phone = item.Phone;
                    string roleNames = "";
                    foreach (var model in temp)
                    {
                        roleNames += RoleConvert.GetRoleName(model.RoleId) + ",";
                        senddto.ConstName = model.Name;
                        senddto.RoleId = model.RoleId;
                        bll.DelDispatch(senddto);
                    }
                    senddto.RolesName = roleNames.Length > 0 ? roleNames.Substring(0, roleNames.Length - 1) : "";
                    sendList.Add(senddto);
                }
            }

            //发送
            string content = System.Configuration.ConfigurationSettings.AppSettings["content"];
            foreach (var temp in sendList)
            {
                string message = string.Format(content, temp.ConstName, temp.RolesName);
                var smsState = c.SendMessages(temp.Phone, message);
            }
        }
    }
}
