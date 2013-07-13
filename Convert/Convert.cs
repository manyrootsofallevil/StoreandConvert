using HostFeed;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Convert
{
    public partial class Convert : ServiceBase
    {

        DateTime nextRunTime = DateTime.MaxValue;


        public Convert()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            nextRunTime = setNextRunTime();
            ticker.Enabled = true;
        }

        protected override void OnStop()
        {
            ticker.Enabled = false;
        }

        private DateTime setNextRunTime()
        {
            DateTime runTime = DateTime.Parse(ConfigurationManager.AppSettings["runtime"]);

            //return runTime.AddDays(int.Parse(ConfigurationManager.AppSettings["Interval"]));

            //TODO: need to write the last runtime to the config file? so that the next run time can be calculated from there, 
            // as otherwise starting and stopping would mess it up
        }

        private void ticker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now > nextRunTime)
            {
                Host.Start();

                ConvertUserExternalTool();

                nextRunTime = setNextRunTime();

                Host.Stop();
            }
        }

        //TODO: Fix this. ta da!!!!
        private void ConvertUserExternalTool()
        {
            if (this.ItemList.Count() > 0)
            {
                string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                ImageModifier image = new ImageModifier(string.Format("{0}\\Images\\beastie.jpg", exePath));

                if (System.IO.Path.GetFileNameWithoutExtension(saveFileName).Equals(
                    DateTime.Now.ToString("yyyyMMdd"), StringComparison.InvariantCultureIgnoreCase))
                {
                    image.WriteMessageToImage(DateTime.Now.ToLongDateString(), "modifiedbeastie.jpg");
                }
                else
                {
                    image.WriteMessageToImage(System.IO.Path.GetFileNameWithoutExtension(saveFileName), DateTime.Now.ToLongDateString(), "modifiedbeastie.jpg");
                }

                CompletedConversion = Common.RunExternalApplication(AppType.EbookConverter,
                           string.Format("{0} \"{1}\" --authors {2} --title {3} --cover {4}",
                           ConfigurationManager.AppSettings["newsrecipepath"],
                                                              saveFileName,
                           ConfigurationManager.AppSettings["author"],
                           DateTime.Now.ToString("yyyyMMdd"),
                           string.Format("\"{0}\\modifiedbeastie.jpg\"", exePath)));

                //Should really migrate the whole thing to use MVVM.
                if (CompletedConversion)
                {
                    Emailer.Hotmail(string.Format("{0}", saveFileName));

                    Trace.WriteLine("Converted book can be found here {1}{0}.mobi", DateTime.Now.ToString("yyyyMMdd")
                         , ConfigurationManager.AppSettings["savefilepath"]);
                }



            }
        }
    }
}
