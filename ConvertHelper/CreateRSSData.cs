using HostFeed;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertHelper
{
    public static class CreateRSSData
    {
        public static void CreateRSSItems(List<HTMLPage> pages)
        {
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("Root"));

            foreach (HTMLPage page in pages)
            {
                string title = page.FileName == null ? page.Title : page.FileName;

                xdoc.Root.Add(new XElement("RssItem", new XAttribute("title", title)
                , new XAttribute("content", page.Content), new XAttribute("URI", page.Url)));
            }

            xdoc.Save(ConfigurationManager.AppSettings["rsssource"]);
        }
    }
}
