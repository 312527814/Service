using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Comm
{
    public class SmsMessage
    {
        string sendurl = "http://api.itrigo.net/mt.jsp?";
        string ac = "guangjiagufen"; //用户名
        string authkey = "a123456"; //密钥
        string cgid = "5390"; //通道组编号
        string csid = "1";
        public string SendMessages(string phoneNumber, string constName, int phase, int role = 3)
        {
            string m = phoneNumber; //发送号码
            //string strnumber = GetNumber();
            string postReturn = "";
            string reg = "";
            WebClient client = new WebClient();
            try
            {
                StringBuilder sbTemp = new StringBuilder();
                //POST 传值
                //String ct = HttpUtility.UrlEncode(content, Encoding.GetEncoding("gbk"));
                String content = "";

                if (role == 3)
                {
                    content = HttpUtility.UrlEncode(string.Format(@"【鹅兔鹅】尊敬的客户您好:鹅兔鹅一直致力于为您提供优质的服务为核心，您的项目已经进展到第[{0}]周，请您对该阶段服务质量进行评价，您的评价对我们很重要。", phase),
                    System.Text.Encoding.GetEncoding("gbk"));
                }
                else
                {
                    content = HttpUtility.UrlEncode(string.Format(@"【鹅兔鹅】您的[{0}]项目在第{1}周，尚未评价！请及时联系业主帮您评价吧，如不评价对您的的积分排名会有影响哟", constName, phase),
                    System.Text.Encoding.GetEncoding("gbk"));
                }

                String ct = content;
                reg = client.DownloadString("http://api.itrigo.net/mt.jsp?cpName=" + ac + "&cpPwd=" + authkey + "&phones=" + phoneNumber + "&msg=" + ct);

            }
            catch (Exception)
            {
                throw;
            }
            return reg;
        }

        public string SendMessages(string phoneNumber, string content)
        {
            string m = phoneNumber; //发送号码
            //string strnumber = GetNumber();
            string reg = "";
            WebClient client = new WebClient();
            try
            {
                content = HttpUtility.UrlEncode(string.Format(content),
                System.Text.Encoding.GetEncoding("gbk"));
                
                String ct = content;
                reg = client.DownloadString("http://api.itrigo.net/mt.jsp?cpName=" + ac + "&cpPwd=" + authkey + "&phones=" + phoneNumber + "&msg=" + ct);

            }
            catch (Exception)
            {
                throw;
            }
            return reg;
        }
    }
}
