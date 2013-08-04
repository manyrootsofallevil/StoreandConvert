using ConvertHelper;
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
            ticker.Interval = 3000;
#endif
            //TODO: No idea why this is not working first time, so leaving it here as a backup.
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

                RssFeedHost host = new RssFeedHost();

                host.Start();

                List<HTMLPage> webPages = FileHandler.GetFiles(ConfigurationManager.AppSettings["StoreDirectory"]);

                CreateRSSData.CreateRSSItems(webPages);

                Converter.ConvertUsingExternalTool();

                FileHandler.MoveFiles(ConfigurationManager.AppSettings["StoreDirectory"], ConfigurationManager.AppSettings["BackupDirectory"]);

                nextRunTime = SetNextRunTime();

                host.Stop();

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

    }
}
