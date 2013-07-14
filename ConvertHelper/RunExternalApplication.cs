using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHelper
{
    public enum AppType { Browser, EbookConverter, EbookViewer };

    public class RunExternalApplication
    {
        public static bool RunExternalApp(AppType app, string arguments)
        {

            Trace.TraceInformation(string.Format("RunExternalApplication {0} {1}.", app.ToString(), arguments));

            bool output = false;

            switch (app)
            {
                case AppType.Browser:
                    Process.Start(ConfigurationManager.AppSettings["Browser"], arguments);
                    break;
                case AppType.EbookConverter:
                    //Process.Start(, arguments);
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = ConfigurationManager.AppSettings["EbookConverter"],
                            Arguments = arguments,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    proc.Start();
                    while (!proc.StandardOutput.EndOfStream)
                    {
                        string line = proc.StandardOutput.ReadLine().ToLower();

                        Trace.TraceInformation(line, "HTMLJoiner");

                        if (line.Contains("output saved"))
                        {
                            output = true;
                        }
                    }
                    break;
                case AppType.EbookViewer:
                    Process.Start(ConfigurationManager.AppSettings["EbookViewer"], arguments);
                    break;



            }
            //TODO: Have a look at this to get the ouput from ebook converter process
            return output;
        }
    }
}
