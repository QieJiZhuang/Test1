using System;

using System.Text;

using System.Net;

using System.IO;

using System.Text.RegularExpressions;

namespace ConsoleApplication1

{

    class Program
    //getIndexDemandList?orderType=2&pageNum=1&pageSize=16
    {

        static void Main(string[] args)
        {
            string str = SendHttpPost("http://www.sipop.cn/module/gate/supplyAndDemand/index.html#/supplyList","");
            Console.WriteLine(str);
            Console.ReadKey();
        }

        public static string SendHttpPost(string url,string paraJsonStr)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content_Type", "application/x-www-form-urlencoded");
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(paraJsonStr);
            byte[] responseData = webClient.UploadData(url,"GET",postData);
            string returnStr = System.Text.Encoding.UTF8.GetString(responseData);
            return returnStr;
        }

    }

}

