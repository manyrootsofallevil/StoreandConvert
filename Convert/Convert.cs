using ConvertHelper;
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
            //Debugger.Launch();
            //TODO: why is this failing to work???
            EncryptAppConfig.EncryptAppSettings();


            nextRunTime = GetNextRunTime();
            //ticker.Interval = 10000;
            ticker.Enabled = true;

            ConfigurationManager.RefreshSection("configuration");
           
        }

        protected override void OnStop()
        {
            ticker.Enabled = false;
        }

        private void ticker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now > nextRunTime)
            {
                ticker.Enabled = false;

                Host.Start();

                List<HTMLPage> webPages = FileHandler.GetFiles(ConfigurationManager.AppSettings["StoreDirectory"]);

                CreateRSSData.CreateRSSItems(webPages);

                ConvertUsingExternalTool();

                FileHandler.MoveFiles(ConfigurationManager.AppSettings["StoreDirectory"], ConfigurationManager.AppSettings["BackupDirectory"]);

                nextRunTime = SetNextRunTime();

                Host.Stop();

                ticker.Enabled = true;
            }
        }

        private DateTime GetNextRunTime()
        {
            DateTime runTime, result;

            if (DateTime.TryParse(ConfigurationManager.AppSettings["nextruntime"], out runTime))
            {
                result = runTime;
            }
            else
            {
                result = SetNextRunTime();
            }

            return result;
        }

        private DateTime SetNextRunTime()
        {
            DateTime runTime = DateTime.Parse(ConfigurationManager.AppSettings["runtime"]);
            runTime = runTime.AddDays(int.Parse(ConfigurationManager.AppSettings["interval"]));

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            config.AppSettings.Settings["NextRunTime"].Value = runTime.ToString();
            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            return runTime;

        }

        private void ConvertUsingExternalTool()
        {
            bool completedConversion;
            //TODO: handle trailing slash\\
            string saveFileName = string.Format("{0}{1}",ConfigurationManager.AppSettings["savefilepath"],
                string.Format(ConfigurationManager.AppSettings["OutputFileNamePattern"], DateTime.Now));

            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


            //TODO: Get this working.
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

            //completedConversion = RunExternalApplication.RunExternalApp(AppType.EbookConverter,
            //           string.Format("{0} \"{1}\" --authors {2} --title {3} --cover {4}",
            //           ConfigurationManager.AppSettings["newsrecipepath"],
            //                                              saveFileName,
            //           ConfigurationManager.AppSettings["author"],
            //           DateTime.Now.ToString("dd - MMM - yyyy"),
            //           string.Format("\"{0}\\modifiedbeastie.jpg\"", exePath)));
            completedConversion = RunExternalApplication.RunExternalApp(AppType.EbookConverter,
                       string.Format("{0} \"{1}\" --authors {2} --title {3}",
                       ConfigurationManager.AppSettings["newsrecipepath"],
                                                          saveFileName,
                       ConfigurationManager.AppSettings["author"],
                       DateTime.Now.ToString("\"dd-MMM-yyyy\"")));

            if (completedConversion)
            {
                Emailer.Hotmail(string.Format("{0}", saveFileName));

                Trace.WriteLine(string.Format("Converted book can be found here {1}{0}.mobi", DateTime.Now.ToString("yyyyMMdd")
                     , ConfigurationManager.AppSettings["savefilepath"]));
            }
        }

    }
}
