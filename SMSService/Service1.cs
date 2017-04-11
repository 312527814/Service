using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Comm;
using Interfaces;
using Services;
using AutoMapper;
using Model;

namespace SMSService
{
    /// <summary>
    /// 项目评价阶段提醒
    /// </summary>
    public partial class Service1 : ServiceBase
    {

        System.Timers.Timer timer = new System.Timers.Timer(86400000);//3600000
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //执行频率  毫秒为单位
            //string timeNumber = System.Configuration.ConfigurationSettings.AppSettings["TimeNumber"].ToString();
            this.timer.Interval = 1000;
            //定时启动
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            timer.Start();
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string TimePhase = System.Configuration.ConfigurationSettings.AppSettings["TimePhase"].ToString();
            if (DateTime.Now.ToString("HH:mm:ss") == TimePhase)
            {
                IConstructInterface construct = new ConstructService(); ;
                var result = construct.GetConstruct();
                foreach (var model in result)
                {
                    SmsMessage c = new SmsMessage();
                    var smsState = c.SendMessages(model.Phone, model.ConstName, model.Phase.Value, model.Role);

                    //SmsMessage c = new SmsMessage();
                    //var smsState = "0";//c.SendMessages(model.Phone, model.ConstName, model.Phase.Value, model.Role);
                    
                    if (smsState == "0")
                    {
                        Mapper.Initialize(x => x.CreateMap<SMSEvalLogDTO, SMSEvalLog>());
                        SMSEvalLog sms = Mapper.Map<SMSEvalLogDTO, SMSEvalLog>(model);
                        sms.CreateTime = DateTime.Now;

                        construct.AddSMSLog(sms);
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
