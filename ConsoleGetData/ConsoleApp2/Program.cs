using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://www.sipop.cn/module/gate/supplyAndDemand/index.html#/supplyList";
            //string url = "https://fabiaoqing.com/biaoqing";

            //HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://www.sipop.cn/module/gate/supplyAndDemand/index.html#/supplyList");
            //Request.Timeout = 200 * 1000;//请求超时。
            //Request.AllowAutoRedirect = true; //网页自动跳转。
            //Request.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";//伪装成谷歌爬虫。
            //Request.Method = "POST"; //获取数据的方法。GET
            //                        //Request.Method = "POST";//POST
            //                        //Request.ContentType = "application/json";上传的格式JSON
            //Request.KeepAlive = true; //保持
            //HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            //4.5版本以上可使用：ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            string Htmlstring = string.Empty;
            using (StreamReader sReader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8))
            {
                 Htmlstring = sReader.ReadToEnd();
            }
            Console.WriteLine(Htmlstring);
            //Console.WriteLine(1024+996);
            //int a = 1;
            //int b = 4;
            //Console.WriteLine(a+=b);
            Console.ReadKey();
        }
    }
}
