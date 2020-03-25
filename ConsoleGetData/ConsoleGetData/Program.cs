using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleGetData
{
    class Program
    {
        static void Main(string[] args)
        {


            //WebPage webInfo = new WebPage("http://www.cecport.com/en/firefly/solution/list.jhtml");
            
            WebPage webInfo2 = new WebPage("http://www.cecport.com/en/firefly/solution/list.jhtml?typeFirst=&style=&pageNo=2");
            WebPage webInfo = new WebPage("http://www.sipop.cn/module/gate/supplyAndDemand/index.html#/supplyList");

            var html = webInfo.M_html;//抓取带HTML标签的网页
            //var html = webInfo.Context;//抓取不带HTML标签的纯文本的内容
            //Regex reg = new Regex("<a href=\"(/en/firefly/solution/list.jhtml\\?typeFirst=.*?&amp;style=&amp;pageNo=.*?)\" title=\"(.*?)\".*?>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            //Regex reg = new Regex("<a href=\"(/en/firefly/solution/list.jhtml\\?typeFirst=.*?&amp;style=&amp;pageNo=.*?)\" title=\"(.*?)\".*?>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);


            Regex reg = new Regex("< img data - v - 099d563b = '' src = '//.*?' alt = ''>", RegexOptions.IgnoreCase | RegexOptions.Singleline);


                     var matchs = reg.Matches(html);
            var apps = new Dictionary<string, string>();

            foreach (Match item in matchs)
            {
                var key = item.Groups[2].ToString();   //取得正则表达式里第二个括号里面的值
                var value = item.Groups[1].ToString(); //取得正则表达式里第一个括号里面的值
                if (key == "All")
                {
                    continue;
                }
                if (key.Contains("&amp;"))
                {
                    key = key.Replace("&amp;", "&");
                }
                if (value.Contains("&amp;"))
                {
                    value = value.Replace("&amp;", "&");
                }
                apps.Add(key, "http://www.cecport.com" + value);
            }

            reg = new Regex("<a href=\"/en/firefly/solution/list.jhtml\\?typeFirst=&amp;style=(.*?)&amp;pageNo=.*?\" title=\"(.*?)\".*?>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            matchs = reg.Matches(html);//获得所有符合正则表达式条件的匹配项

            var typeApps = new Dictionary<string, string>();
            foreach (Match item in matchs)
            {
                var key = item.Groups[2].ToString();   //取得正则表达式里第二个括号里面的值
                var value = item.Groups[1].ToString(); //取得正则表达式里第一个括号里面的值
                if (key == "All")
                {
                    continue;
                }
                if (key.Contains("&amp;"))
                {
                    key = key.Replace("&amp;", "&");
                }
                if (value.Contains("&amp;"))
                {
                    value = value.Replace("&amp;", "&");
                }
                typeApps.Add(key, value);
            }

            for (int i = 0; i < apps.Count; i++)
            {
                for (int j = 0; j < typeApps.Count; j++)
                {
                    var href1 = apps.ToArray()[i].Value;
                    var href2 = typeApps.ToArray()[j].Value;

                    string href = href1.Replace("style=", "style=" + href2);
                    webInfo = new WebPage(href);
                    reg = new Regex("<dl class=\"sol-item\">.*?<h3>.*?<a href=\"(/en/firefly/solution/.*?/detail.jhtml)\".*?>(.*?)</a>.*?</h3>.*?</dl>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    html = webInfo.M_html;
                    matchs = reg.Matches(html);//获得所有符合正则表达式条件的匹配项
                    if (matchs.Count == 0)
                    {
                        continue;
                    }
                    var titleApps = new Dictionary<string, string>();
                    foreach (Match item in matchs)
                    {
                        var key = item.Groups[2].ToString();   //取得正则表达式里第二个括号里面的值
                        var value = item.Groups[1].ToString(); //取得正则表达式里第一个括号里面的值
                        if (key.Contains("&amp;"))
                        {
                            key = key.Replace("&amp;", "&");
                        }
                        if (value.Contains("&amp;"))
                        {
                            value = value.Replace("&amp;", "&");
                        }
                        titleApps.Add(key, "http://www.cecport.com" + value);
                        //href = titleApps[key]; //跳转详情页的路径
                        //WebClient MyWebClient = new WebClient();
                        //MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                        //Byte[] pageData = MyWebClient.DownloadData(href); //从指定网站下载数据
                        //string pageHtml = Encoding.UTF8.GetString(pageData);

                        //href = titleApps[key]; //跳转详情页的路径
                        //WebClient wc = new WebClient();
                        //wc.Encoding = Encoding.UTF8;
                        ////以字符串的形式返回数据
                        //string html = wc.DownloadString(href);
                       
                        href = titleApps[key]; //跳转详情页的路径
                        webInfo = new WebPage(href);
                        reg = new Regex("<div class=\"sol-keys mt50 f14 mb10\">.*?</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        html = webInfo.M_html;
                        matchs = reg.Matches(html);
                    }
                    
                }
            }
            Console.ReadKey();
        }
    }
}
