using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHelper
{
    public static class Converter
    {
        public static void ConvertUsingExternalTool()
        {
            bool completedConversion;
            List<string> images = new List<string>();

            string saveFileName = string.Format(@"{0}\{1}", ConfigurationManager.AppSettings["savefilepath"].Trim('\\'),
                string.Format(ConfigurationManager.AppSettings["OutputFileNamePattern"], DateTime.Now));

            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (Directory.Exists(ConfigurationManager.AppSettings["imagespath"].Trim('\\')))
            {
                images = Directory.GetFiles(ConfigurationManager.AppSettings["imagespath"].Trim('\\'), "*.*", SearchOption.AllDirectories)
                       .Where(x => x.EndsWith(".jpg") || x.EndsWith(".png") || x.EndsWith(".jpeg")).ToList();
            }

            if (images.Count > 0)
            {
                Random rng = new Random();

                ImageModifier image = new ImageModifier(images[rng.Next(0, images.Count)]);

                Trace.TraceInformation("Using image {0} for cover", image);

                image.WriteMessageToImage(DateTime.Now.ToLongDateString(),
                    string.Format(@"{0}\frontpage.jpg", exePath.Trim('\\')));

                completedConversion = RunExternalApplication.RunExternalApp(AppType.EbookConverter,
                       string.Format("{0} \"{1}\" --authors {2} --title {3} --cover {4}",
                       ConfigurationManager.AppSettings["newsrecipepath"], saveFileName,
                       ConfigurationManager.AppSettings["author"], DateTime.Now.ToString("\"dd-MMM-yyyy\""),
                        string.Format("\"{0}\\frontpage.jpg\"", exePath.Trim('\\'))));
            }
            else
            {
                completedConversion = RunExternalApplication.RunExternalApp(AppType.EbookConverter,
                       string.Format("{0} \"{1}\" --authors {2} --title {3}",
                       ConfigurationManager.AppSettings["newsrecipepath"], saveFileName,
                       ConfigurationManager.AppSettings["author"], DateTime.Now.ToString("\"dd-MMM-yyyy\"")));
            }

            if (completedConversion)
            {
                Emailer.Hotmail(string.Format("{0}", saveFileName));

                Trace.TraceInformation("Converted book can be found here {0}{1}.mobi",
                     ConfigurationManager.AppSettings["savefilepath"], DateTime.Now.ToString("yyyyMMdd"));
            }
        }
    }
}
