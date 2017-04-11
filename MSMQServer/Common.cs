using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MSMQServer
{
    public class Common
    {
        /// <summary>
        /// 通过Create方法创建使用指定路径的新消息队列
        /// </summary>
        /// <param name="queuePath"></param>
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
                    //Console.WriteLine(queuePath + "已经存在！");
                }
            }
            catch (MessageQueueException e)
            {
                //Console.WriteLine(e.Message);
            }
        }
    }
}
