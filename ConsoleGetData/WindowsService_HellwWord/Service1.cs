using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService_HellwWord
{
    public partial class Service1 : ServiceBase
    {
        //记录到event log中，地址是 C:\Windows\System32\winevt\Logs (双击查看即可，文件名为MyNewLog)
        private static EventLog eventLog1;
        private int eventId = 1;

        public Service1()
        {
            InitializeComponent();

            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart.");
            log("In OnStart.");
            TimeDo();
            // Set up a timer that triggers every minute. 设置定时器
            //Timer timer = new Timer();
            //timer.Interval = 001 * *? ; // 60 seconds 60秒执行一次
            //timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            //timer.Start();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop.");
            log("In OnStop.");
        }

        /// <summary>
        /// 继续服务
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
            log("In OnContinue.");
        }

        /// <summary>
        /// 定时器中定时执行的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimer(object sender, ElapsedEventArgs args)
        {

            if (Num()>0)
            {
                //eventLog1.WriteEntry("执行成功", EventLogEntryType.Information, eventId++);
                log("执行成功！");
            }
            else
            {
                log("执行失败！");
            }
            // TODO: Insert monitoring activities here.
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);

            

            //log("the timer");
        }

        

        public int Num()
        {
            return 2;
        }


        /// <summary>
        /// 记录到指定路径：D:\log.txt
        /// </summary>
        /// <param name="message"></param>
        private static void log(string message)
        {
            using (FileStream stream = new FileStream("D:\\log.txt", FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now}:{message}");
            }
        }



        ///// <summary>  
        ///// 定时器  
        ///// </summary>  
        public void TimeDo()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimeEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
        }
        ///// <summary>  
        ///// 定时器触发事件  
        ///// </summary>  
        ///// <param name="source"></param>  
        ///// <param name="e"></param>  
        private static void TimeEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            int iHour = 2;
            int iMinute = 00;
            int iSecond = 1;

            if (intSecond == iSecond)
            {
                log("没苗");
            }
            // 设置　每天的１０：３０：００开始执行程序  
            if (intHour == iHour && intMinute == iMinute && intSecond == iSecond)
            {
                Console.WriteLine("在每天2点分开始执行！");
            }
        }




        }
}
