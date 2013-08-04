using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreAndConvert.WCFService
{
    public class WebPage
    {
        public string StoreDirectory { get; set; }

        public WebPage(string storeDirectory)
        {
            StoreDirectory = storeDirectory;
        }

        /// <summary>
        /// Store the argument url to a file on directory StoreDirectory.
        /// The filename will be the days date formated as yyyyMMdd.
        /// Web page title and content description will be stored if they can be found.
        /// </summary>
        /// <param name="url">Url to store</param>
        /// <param name="title">Document Title</param>
        public void Store(string url, string title)
        {
            Trace.WriteLine(string.Format("Storing: {0}.", url));

            XDocument xdoc = null;
            string fileName = Path.Combine(StoreDirectory, string.Format("{0:yyyyMMdd}.xml", DateTime.Now));

            //I don't think content is used for anything. The periodicals use the url and title only
            if (File.Exists(fileName))
            {
                xdoc = XDocument.Load(fileName);
                xdoc.Root.Add(new XElement("RssItem", new XAttribute("title", title)
                , new XAttribute("content", string.Empty), new XAttribute("URI", url)));
            }
            else
            {
                xdoc = new XDocument();
                xdoc.Add(new XElement("Root"));

                xdoc.Root.Add(new XElement("RssItem", new XAttribute("title", title)
                           , new XAttribute("content", string.Empty), new XAttribute("URI", url)));
            }
            //oh, I like unnecessary complications.
            using (StreamWriter outStream = System.IO.File.CreateText(fileName))
            {
                xdoc.Save(outStream);
            }

            Trace.WriteLine(string.Format("Stored: {0} on {1}.", url, fileName));
        }

    }
}
