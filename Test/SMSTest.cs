using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Comm;
using Interfaces;
using Services;
using Model;
using System.Messaging;
using Newtonsoft.Json;
using System.Threading;
using Model.DTO;
using AutoMapper;

namespace Test
{
    public partial class SMSTest : Form
    {
        public SMSTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string TimePhase = System.Configuration.ConfigurationSettings.AppSettings["TimePhase"].ToString();
            if (DateTime.Now.ToString("HH:mm:ss") == TimePhase)
            {
                IConstructInterface construct = new ConstructService(); ;
                var result = construct.GetConstruct();
                foreach (var model in result)
                {
                    SmsMessage c = new SmsMessage();
                    var smsState = "0";//c.SendMessages(model.Phone, model.ConstName, model.Phase.Value, model.Role);

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

        private void button2_Click(object sender, EventArgs e)
        {
            List<TempList> t = new List<TempList>();
            t.Add(new TempList() { Days = 1, ProcessId = 2, Sort = 1 });
            t.Add(new TempList() { Days = 2, ProcessId = 1, Sort = 1 });
            t.Add(new TempList() { Days = 3, ProcessId = 3, Sort = 1 });

            t.Add(new TempList() { Days = 1, ProcessId = 4, Sort = 2 });
            t.Add(new TempList() { Days = 2, ProcessId = 5, Sort = 2 });
            t.Add(new TempList() { Days = 3, ProcessId = 6, Sort = 2 });

            List<TempList> r = new List<TempList>();
            r.Add(new TempList() { Days = 1, ProcessId = 2, Sort = 1 });
            r.Add(new TempList() { Days = 2, ProcessId = 1, Sort = 1 });

            List<TempList> bzInputList = new List<TempList>();
            foreach (var temp in r)
            {
                var list = t.Where(p => p.ProcessId == temp.ProcessId);
                if (list.Any())
                {
                    bzInputList.Add(r.First());
                }
            }

            //分组
            //var ls = t.GroupBy(a => new { a.Sort }).Select(g => new TempList() { Sort = g.Key.Sort, Days = g.Max(item => item.Days) }).ToList();
            //包含

            //var ls = t.Where(p => p.ProcessId.CompareTo() r.Contains(p.ProcessId)).ToList();
        }

        private static int ThreadNumber = 20;  //2个线程序
        private static Thread[] ThreadArray = new Thread[ThreadNumber];

        MessageQueue myQueue = new MessageQueue(".\\private$\\RobOrderQueue");   //连接到本地的队列
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Createqueue(@".\private$\RobOrderQueue");
                for (int i = 0; i < ThreadArray.Length; i++)
                {
                    ThreadArray[i] = new Thread(delegate () { Start(i); });
                    ThreadArray[i].Start();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<DataResullt> CompData(DateTime StartTime, DateTime EndTime)
        {
            List<DataResullt> DataList = new List<DataResullt>();

            int days = (EndTime - StartTime).Days + 1;
            int month = StartTime.Month;
            List<int> daylist = new List<int>();
            string ym = StartTime.ToString("yyyy年MM月");
            for (int i = 0; i < days; i++)
            {
                DateTime time = StartTime.AddDays(i);
                if (time.Month != month)
                {
                    DataResullt model = new DataResullt();
                    model.YM = ym;
                    model.Days = daylist;
                    DataList.Add(model);

                    ym = time.ToString("yyyy年MM月");
                    month = time.Month;
                    daylist = new List<int>();
                }
                daylist.Add(time.Day);
            }
            if (daylist.Count > 0)
            {
                DataResullt model = new DataResullt();
                model.YM = ym;
                model.Days = daylist;
                DataList.Add(model);
            }
            return DataList;
        }

        private void Start(int threadIndex)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    System.Messaging.Message myMessage = new System.Messaging.Message();

                    TempList model = new TempList();
                    model.ProcessId = i + 1;
                    model.Days = 1;
                    model.Sort = threadIndex;

                    myMessage.Body = JsonConvert.SerializeObject(model);
                    myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                    //发送消息到队列中
                    myQueue.Send(myMessage);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Createqueue(@".\private$\RobOrderQueue");
                System.Messaging.Message myMessage = new System.Messaging.Message();

                Model.InputDispatch model = new Model.InputDispatch();
                model.ConstId = 7091;
                model.RoleId = 9;
                model.UserId = 13641;

                myMessage.Body = JsonConvert.SerializeObject(model);
                myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                //发送消息到队列中
                myQueue.Send(myMessage);
            }
            catch (Exception ex)
            {

            }
        }

        public static void Createqueue(string queuePath)
        {
            try
            {
                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(@".\private$\RobOrderQueue");
                }
                else
                {
                    Console.WriteLine(queuePath + "已经存在！");
                }
            }
            catch (MessageQueueException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageQueue myQueue = new MessageQueue(".\\private$\\RobOrderQueue");
            ITestInterface _test = new TestService();

            while (true)
            {
                //连接到本地队列
                System.Messaging.Message message = myQueue.Receive();
                XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                message.Formatter = formatter;
                string jsonData = message.Body.ToString();
                Model.Test test = JsonConvert.DeserializeObject<Model.Test>(jsonData);

                var result = _test.InsertTest(test);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<DataResullt> result = CompData(DateTime.Parse("2016-12-12 00:00:00.000"), DateTime.Parse("2016-12-13 00:00:00.000"));


            int r = 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //const string queueName = @"FormatName:DIRECT=TCP:123.56.129.104\private$\RobOrderQueue";
            const string queueName = @".\private$\RobOrderQueue";

            Model.InputDispatch model = new Model.InputDispatch();
            model.ConstId = 3826;
            model.RoleId = 11;
            model.UserId = 26154;  //13640  13501 13641

            try
            {
                //将异步消息发往指定的消息队列  
                using (MessageQueue msmq = new MessageQueue(queueName))
                {
                    msmq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    System.Messaging.Message msg = new System.Messaging.Message() { Label = "业务模块异步消息", Body = JsonConvert.SerializeObject(model) };
                    msmq.Send(msg);
                }
            }
            catch (Exception ee)
            {
                //Console.WriteLine(String.Format("消息发送失败，原因是：{0}", ee.Message));
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            IDispatchInterface s = new DispatchService();
            Model.InputDispatch model = new Model.InputDispatch();

            model.ConstId = 7091;
            model.RoleId = 9;
            model.UserId = 13640;  //13640  13501 13641
            s.InsertDispatch(model);
        }

        private int start = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["start"]);
        private string content = System.Configuration.ConfigurationSettings.AppSettings["content"];

        private void button9_Click(object sender, EventArgs e)
        {
            ITestInterface _test = new TestService();
            Model.Test model1 = new Model.Test();
            model1.ProcessId = 3 + 1;
            model1.Days = 1;
            model1.Sort = 33;
            _test.InsertTest(model1);

            //IDispatchInterface bll = new DispatchService();
            //SmsMessage c = new SmsMessage();
            //List<OrdDispatchDto> list = bll.GetDispatch();

            ////var ls = t.GroupBy(a => new { a.Sort }).Select(g => new TempList() { Sort = g.Key.Sort, Days = g.Max(item => item.Days) }).ToList();
            //List<GroupDispatchDto> newlist = list.GroupBy(p => new { p.ConstId, p.Phone }).Select(g => new GroupDispatchDto() { ConstId = g.Key.ConstId, Phone = g.Key.Phone }).ToList();
            //List<SendSmsDto> sendList = new List<SendSmsDto>();
            //foreach (var item in newlist)
            //{
            //    var temp = list.Where(p => p.ConstId == item.ConstId).ToList();
            //    if (temp[0].CreateTime.AddHours(24) < DateTime.Now)
            //    {
            //        SendSmsDto senddto = new SendSmsDto();
            //        senddto.ConstId = item.ConstId;
            //        senddto.Phone = item.Phone;
            //        string roleNames = "";
            //        foreach (var model in temp)
            //        {
            //            roleNames += RoleConvert.GetRoleName(model.RoleId) + ",";
            //            senddto.ConstName = model.Name;
            //            senddto.RoleId = model.RoleId;
            //            bll.DelDispatch(senddto);
            //        }
            //        senddto.RolesName = roleNames.Length > 0 ? roleNames.Substring(0, roleNames.Length - 1) : "";
            //        sendList.Add(senddto);
            //    }
            //}

            ////发送   给监理
            //string content = System.Configuration.ConfigurationSettings.AppSettings["content"];
            //foreach (var temp in sendList)
            //{
            //    //string message = string.Format(content, temp.ConstName, temp.RolesName);
            //    //var smsState = c.SendMessages(temp.Phone, message);
            //}
        }
    }


    public class DataResullt
    {
        public string YM { get; set; }

        public List<int> Days { get; set; }
    }

    public class TempList
    {
        public int ProcessId { get; set; }

        public int Days { get; set; }

        public int Sort { get; set; }
    }
}
