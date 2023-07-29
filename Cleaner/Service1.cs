using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace Cleaner
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            this.timer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;//1000 * 60 * 60 * 2;
            this.timer.Elapsed += new ElapsedEventHandler(this.CleanerAllData);
            //this.timer.Enabled = true;
            this.timer.Start();
        }

        public void CleanerAllData(object sender, ElapsedEventArgs e)
        {
            //Main Path
            string MainPath = "C:\\";
            //All Full Path list
            List<string> files = new List<string> { MainPath + "Windows\\Temp", MainPath + "Users\\ADMINI~1\\AppData\\Local\\Temp", MainPath + "Windows\\Prefetch" };
            //Delete All Folder data
            foreach (var file in files)
            {
                DirectoryInfo dir = new DirectoryInfo(file);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception) { } // Ignore all exceptions
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    try
                    {
                        di.Delete();
                    }
                    catch (Exception) { } // Ignore all exceptions
                }
            }
            //This function call tree and clean
            Process.Start(MainPath + "Windows\\system32\\tree.com"); Process.Start("tree.com");
            //System ShutDown only Saturady
            if (DateTime.Now.ToString("dddd").ToUpper() == "SATURDAY" && DateTime.Now.ToString("hhtt").ToUpper() != "12PM" && Convert.ToInt32(DateTime.Now.ToString("hhmm")) > 0900 && DateTime.Now.ToString("tt").ToUpper() == "PM")
            {
                Process.Start("shutdown.exe", "-s");
            }
        }
        protected override void OnStop()
        {
            timer.Stop();
            timer = null;
        }
    }
}
