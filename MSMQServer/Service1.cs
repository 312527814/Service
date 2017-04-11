using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Model;
using Newtonsoft.Json;
using Services;
using System.Threading;

namespace MSMQServer
{
    /// <summary>
    ///  抢单队列处理
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread t = new Thread(Start);
            t.Start();
        }

        public void Start()
        {
            MessageQueue myQueue = new MessageQueue(".\\private$\\RobOrderQueue");
            IDispatchInterface servier = new DispatchService();

            while (true)
            {
                //连接到本地队列
                System.Messaging.Message message = myQueue.Receive();
                XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                message.Formatter = formatter;
                string jsonData = message.Body.ToString();
                Model.InputDispatch model = JsonConvert.DeserializeObject<Model.InputDispatch>(jsonData);

                servier.InsertDispatch(model);

            }
        }

        protected override void OnStop()
        {
        }
    }
}
