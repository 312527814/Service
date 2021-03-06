﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//新添加命名空间  
using System.Net;
using System.IO;
using System.Text.RegularExpressions;


namespace WebBrowserCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text.Trim());         //显示网页
        }

        //定义num记录listBox2中获取到的图片URL个数
        public int num = 0;
        //点击"获取"按钮
        private void button2_Click(object sender, EventArgs e)
        {
            HtmlElement html = webBrowser1.Document.Body;      //定义HTML元素
            string str = html.OuterHtml;                       //获取当前元素的HTML代码
            MatchCollection matches;                           //定义正则表达式匹配集合
            //清空
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            //获取
            try
            {
                //正则表达式获取<a href></a>内容url
                matches = Regex.Matches(str, "<a href=\"([^\"]*?)\".*?>(.*?)</a>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    listBox1.Items.Add(match.Value.ToString());
                }
                //正则表达式获取<img src=>图片url
                matches = Regex.Matches(str, @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    listBox2.Items.Add(match.Value.ToString());
                }
                //记录图片总数
                num = listBox2.Items.Count;

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);    //异常处理
            }
        }

        //点击"下载"实现下载图片
        private void button3_Click(object sender, EventArgs e)
        {
            string imgsrc = string.Empty;             //定义
            //循环下载
            for (int j = 0; j < num; j++)
            {
                string content = listBox2.Items[j].ToString();    //获取图片url
                Regex reg = new Regex(@"<img.*?src=""(?<src>[^""]*)""[^>]*>", RegexOptions.IgnoreCase);
                MatchCollection mc = reg.Matches(content);        //设定要查找的字符串
                foreach (Match m in mc)
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(m.Groups["src"].Value);    //图片src内容
                        WebResponse response = request.GetResponse();
                        //文件流获取图片操作
                        Stream reader = response.GetResponseStream();
                        string path = "E://" + j.ToString() + ".jpg";                     //图片路径命名 
                        FileStream writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                        byte[] buff = new byte[512];
                        int c = 0;                                                        //实际读取的字节数   
                        while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                        {
                            writer.Write(buff, 0, c);
                        }
                        //释放资源
                        writer.Close();
                        writer.Dispose();
                        reader.Close();
                        reader.Dispose();
                        response.Close();
                        //下载成功
                        listBox2.Items.Add(path + ":图片保存成功!");
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message);
                    }
                }
            }
        }

        /// <summary> 
        /// 取得HTML中所有图片的 URL
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
            {
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            }
            return sUrlList;
        }


        /// <summary>
        /// 获得图片的路径并存放
        /// </summary>
        /// <param name="M_Content">要检索的内容</param>
        /// <returns>IList</returns>
        public static IList<string> GetPicPath(string M_Content)
        {
            IList<string> im = new List<string>();//定义一个泛型字符类
            Regex reg = new Regex(@"<img.*?src=""(?<src>[^""]*)""[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection mc = reg.Matches(M_Content); //设定要查找的字符串
            foreach (Match m in mc)
            {
                im.Add(m.Groups["src"].Value);
            }
            return im;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string url = "http://www.aitaotu.com/guonei/list_1.html";
            WebClient myWebClient = new WebClient();
            byte[] myDataBuffer = myWebClient.DownloadData(url);
            string str = Encoding.UTF8.GetString(myDataBuffer);

            List<Temp> list = new List<Temp>();

            //需要抓取的标签内容
            MatchCollection matches;
            matches = Regex.Matches(str, "<div class=\"item masonry_brick\">([\\w\\W]*?)[张]+</div>", RegexOptions.IgnoreCase);

            MatchCollection matchImgs;
            MatchCollection matchDates;
            MatchCollection shuxing;

            //获取
            try
            {
                foreach (Match match in matches)
                {
                    //图片标题等信息
                    matchImgs = Regex.Matches(match.Value, "<a href=\"([^\"]*?)\".*?><img(.*?)</a>", RegexOptions.IgnoreCase);
                    //日期
                    matchDates = Regex.Matches(match.Value, "(?<=[:\\d])(.)*\\d(?=&)", RegexOptions.IgnoreCase);

                    for (int i = 0; i < matchImgs.Count; i++)
                    {
                        shuxing = Regex.Matches(matchImgs[i].Value, "\"(.*?)\"", RegexOptions.IgnoreCase);
                        Temp t = new Temp();
                        t.Date = Convert.ToDateTime(matchDates[0].Value);
                        t.Url = shuxing[0].Value;
                        t.ImgUrl = shuxing[2].Value;
                        t.Tilte = shuxing[4].Value;
                        list.Add(t);
                        //string temp = matchImgs[i].Value;
                    }
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);    //异常处理
            }
        }
    }

    public class Temp
    {
        public string Url { get; set; }

        public string ImgUrl { get; set; }

        public string Tilte { get; set; }

        public DateTime Date { get; set; }
    }
}
