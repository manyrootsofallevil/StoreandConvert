﻿using ConvertHelper;
using HostFeed;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
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
#if DEBUG
            Debugger.Launch();
            ticker.Interval = 10000;
#endif
            //TODO: No idea why this is not working first time, so leaving it here as a backup.
            //Use .\aspnet_regiis.exe -pef appSettings <app directory path> instead post install.
            EncryptAppConfig.EncryptAppSettings();

            nextRunTime = GetNextRunTime();

            ticker.Enabled = true;

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

                Converter.ConvertUsingExternalTool();

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

        //private void ConvertUsingExternalTool()
        //{
        //    bool completedConversion;

        //    string saveFileName = string.Format(@"{0}\{1}", ConfigurationManager.AppSettings["savefilepath"].Trim('\\'),
        //        string.Format(ConfigurationManager.AppSettings["OutputFileNamePattern"], DateTime.Now));

        //    string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        //    List<string> images = Directory.GetFiles(ConfigurationManager.AppSettings["imagespath"].Trim('\\'),"*.*",SearchOption.AllDirectories)
        //        .Where(x => x.EndsWith(".jpg") || x.EndsWith(".png") || x.EndsWith(".jpeg")).ToList();

        //    if (images.Count > 0)
        //    {
        //        Random rng = new Random();

        //        ImageModifier image = new ImageModifier(images[rng.Next(0, images.Count)]);

        //        image.WriteMessageToImage(Path.GetFileNameWithoutExtension(saveFileName), DateTime.Now.ToLongDateString(),
        //            string.Format(@"{0}\frontpage.jpg", exePath.Trim('\\')));

        //        completedConversion = RunExternalApplication.RunExternalApp(AppType.EbookConverter,
        //               string.Format("{0} \"{1}\" --authors {2} --title {3} --cover {4}",
        //               ConfigurationManager.AppSettings["newsrecipepath"], saveFileName,
        //               ConfigurationManager.AppSettings["author"], DateTime.Now.ToString("\"dd-MMM-yyyy\""),
        //                string.Format("\"{0}\\frontpage.jpg\"", exePath.Trim('\\'))));
        //    }
        //    else
        //    {
        //        completedConversion = RunExternalApplication.RunExternalApp(AppType.EbookConverter,
        //               string.Format("{0} \"{1}\" --authors {2} --title {3}",
        //               ConfigurationManager.AppSettings["newsrecipepath"], saveFileName,
        //               ConfigurationManager.AppSettings["author"], DateTime.Now.ToString("\"dd-MMM-yyyy\"")));
        //    }

        //    if (completedConversion)
        //    {
        //        Emailer.Hotmail(string.Format("{0}", saveFileName));

        //        Trace.TraceInformation("Converted book can be found here {0}{1}.mobi",
        //             ConfigurationManager.AppSettings["savefilepath"], DateTime.Now.ToString("yyyyMMdd"));
        //    }
        //}

    }
}
