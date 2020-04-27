using DTcms.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Model;
using 存储过程;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn1_Click(object sender, EventArgs e)
        {
            decimal pagenum = 1;
            JArray arr = new JArray();
            string FirstUrl = "http://www.sipop.cn/depot/demand/guest/getIndexDemandList?orderType=2&pageNum=1&pageSize=16";
            string Firstresult = "";
            HttpWebRequest Firstreq = (HttpWebRequest)WebRequest.Create(FirstUrl);
            Firstreq.Method = "GET";
            Firstreq.ContentLength = 0;//一定要添加这句
            HttpWebResponse Firstresp = (HttpWebResponse)Firstreq.GetResponse();

            Stream Firststream = Firstresp.GetResponseStream();
            //获取内容
            using (StreamReader reader = new StreamReader(Firststream, Encoding.UTF8))
            {
                Firstresult = reader.ReadToEnd();
            }
            var jsonresult = JObject.Parse(Firstresult);
            string tid_item = jsonresult["data"]["list"].ToString();

            int listcount = int.Parse(jsonresult["data"]["total"].ToString());
            pagenum = Math.Ceiling(Convert.ToDecimal((listcount / 100))) + 1;
            DataSet ds = new DataSet();
            DataTable dtget = null;
            int num = 0;
            DataTable dt = null;
            for (int i = 1; i <= pagenum; i++)
            {
                #region 
                string Url = "http://www.sipop.cn/depot/demand/guest/getIndexDemandList?orderType=2&pageNum=" + i + "&pageSize=16";
                string result = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream();
                //获取内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                }

                var JosnResult = JObject.Parse(result);
                string ListResult = JosnResult["data"]["list"].ToString();

                DataTable dtq = ToDataTableTwo(ListResult);
                DataTable dt2 = UpdateDataTable(dtq);
                if (num == 0)
                {
                    dtget = dt2.Clone();
                    num = 1;
                }
                dtget.Merge(dt2);
                #endregion
            }
            int resultqq = BulkToDB(dtget, "dt_platform_patents");

        }
     
        /// <summary>
        /// 修改数据表DataTable某一列的类型和记录值(正确步骤：1.克隆表结构，2.修改列类型，3.修改记录值，4.返回希望的结果)
        /// </summary>
        /// <param name="argDataTable">数据表DataTable</param>
        /// <returns>数据表DataTable</returns>
        private DataTable UpdateDataTable(DataTable argDataTable)
        {
            DataTable dtResult = new DataTable();
            //克隆表结构
            //设置datatable中列的顺序，达到和SqlServer中的一致，不一致会产生数据错乱
            dtResult = argDataTable.Clone();
            dtResult.Columns["projectId"].SetOrdinal(0);
            dtResult.Columns["projectTitle"].SetOrdinal(1);
            dtResult.Columns["hasRisk"].SetOrdinal(2);
            dtResult.Columns["image"].SetOrdinal(3);
            dtResult.Columns["collectCount"].SetOrdinal(4);
            dtResult.Columns["trades"].SetOrdinal(5);
            dtResult.Columns["projectStatus"].SetOrdinal(6);
            dtResult.Columns["projectNo"].SetOrdinal(7);
            dtResult.Columns["operateMode"].SetOrdinal(8);
            dtResult.Columns["publishedTime"].SetOrdinal(9);
            dtResult.Columns["price"].SetOrdinal(10);
            dtResult.Columns["name"].SetOrdinal(11);
            dtResult.Columns["projectIntro"].SetOrdinal(12);
            dtResult.Columns["viewCount"].SetOrdinal(13);

            foreach (DataRow row in argDataTable.Rows)
            {
                DataRow rowNew = dtResult.NewRow();
                rowNew["projectId"] = row["projectId"];
                //修改记录值
                rowNew["projectTitle"] = row["projectTitle"];
                rowNew["hasRisk"] = row["hasRisk"];
                rowNew["image"] = row["image"];
                rowNew["collectCount"] = row["collectCount"];
                rowNew["trades"] = row["trades"];
                rowNew["projectStatus"] = row["projectStatus"];
                rowNew["projectNo"] = row["projectNo"];
                rowNew["operateMode"] = row["operateMode"];
                rowNew["publishedTime"] = row["publishedTime"].ToString();
                rowNew["price"] = row["price"];
                rowNew["name"] = row["name"];
                rowNew["projectIntro"] = row["projectIntro"];
                rowNew["viewCount"] = row["viewCount"];
                dtResult.Rows.Add(rowNew);
            }
            return dtResult;
        }

        /// <summary>
        /// 时间戳转换成标准时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        private DateTime ConvertToTime(string timeStamp)
        {
            DateTime time = DateTime.Now;
            if (string.IsNullOrEmpty(timeStamp))
            {
                return time;
            }
            try
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000");
                TimeSpan toNow = new TimeSpan(lTime);
                time = dtStart.Add(toNow);
            }
            catch (Exception ex)
            {
                new Exception(ex.ToString());
            }
            return time;
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTableTwo(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {

                            dataRow[current] = dictionary[current];


                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            result = dataTable;
            return result;
        }

        /// <summary>
        /// 获取需求信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn2_Click(object sender, EventArgs e)
        {
            decimal pagenum = 1;
            JArray arr = new JArray();

            string FirstUrl = "http://www.sipop.cn/depot/demand/guest/getProjectBulletin?pageSize=15&pageNum=1";
            Uri uri = new Uri(FirstUrl);
            string Firstresult = "";
            HttpWebRequest Firstreq = (HttpWebRequest)WebRequest.Create(uri);
            Firstreq.Method = "GET";
            Firstreq.ContentLength = 0;//一定要添加这句
            HttpWebResponse Firstresp = (HttpWebResponse)Firstreq.GetResponse();

            Stream Firststream = Firstresp.GetResponseStream();
            //获取内容
            using (StreamReader reader = new StreamReader(Firststream))
            {
                Firstresult = reader.ReadToEnd();
            }
            var aaa = JObject.Parse(Firstresult);
            string tid_item = aaa["data"]["list"].ToString();
            int listcount = int.Parse(aaa["data"]["total"].ToString());
            pagenum = Math.Ceiling(Convert.ToDecimal((listcount / 500)));
            DataSet ds = new DataSet();
            DataTable dtget = null;
            int num = 0;
            for (int i = 1; i <= pagenum; i++)
            {
                string Url = "http://www.sipop.cn/depot/demand/guest/getProjectBulletin?pageSize=100&pageNum=" + i;
                string result = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                //req.Method = "GET";
                //req.ContentLength = 0;//一定要添加这句
                //req.ContentType = "text/html";
                //req.Timeout = 1000000;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream();
                //获取内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                }
              


                var JosnResult = JObject.Parse(result);
                string ListResult = JosnResult["data"]["list"].ToString();


                DataTable dtq = ToDataTableTwo(ListResult);

                DataTable dt2 = UpdateNoticeDataTable(dtq);
                dt2 = UpdateNoticeDataTable(dt2);
                if (num == 0)
                {
                    dtget = dt2.Clone();
                    num = 1;
                }
                dtget.Merge(dt2);
            }
            int resultqq = BulkToDB(dtget, "dt_project_dynamics");
        }

        /// <summary>
        /// 操作转换表格
        /// </summary>
        /// <param name="argDataTable"></param>
        /// <returns></returns>
        private DataTable UpdateNoticeDataTable(DataTable argDataTable)
        {
            DataTable dtResult = new DataTable();
            //克隆表结构
            dtResult = argDataTable.Clone();
            dtResult.Columns["ID"].SetOrdinal(0);
            dtResult.Columns["acitionType"].SetOrdinal(1);
            dtResult.Columns["creatDate"].SetOrdinal(2);
            dtResult.Columns["projectName"].SetOrdinal(3);
            foreach (DataRow row in argDataTable.Rows)
            {
                DataRow rowNew = dtResult.NewRow();
                rowNew["ID"] = row["Id"];
                //修改记录值
                rowNew["acitionType"] = row["acitionType"];
                rowNew["creatDate"] = row["creatDate"].ToString();
                rowNew["projectName"] = row["projectName"];

                dtResult.Rows.Add(rowNew);

            }
            return dtResult;
        }

       /// <summary>
       /// 将数据克隆到表当中
       /// </summary>
       /// <param name="dt">数据源信息</param>
       /// <param name="TableName">数据表名称</param>
       /// <returns></returns>
        public static int BulkToDB(DataTable dt, string TableName)
        {
            int result = 0;
            SqlConnection sqlConn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);

            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlHelper.connectionString);
            bulkCopy.BulkCopyTimeout = 600;
            bulkCopy.DestinationTableName = TableName;
            bulkCopy.BatchSize = dt.Rows.Count;

            try
            {
                sqlConn.Open();
                if (dt != null && dt.Rows.Count != 0)
                    bulkCopy.WriteToServer(dt);
                result = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
            return result;
        }


    }
}