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
        Tuple<string, string> pageDetails = null;

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
        public void Store(string url)
        {
            Trace.WriteLine(string.Format("Storing: {0}.", url));

            XDocument xdoc = null;
            string fileName = string.Format("{0}{1:yyyyMMdd}.xml", StoreDirectory, DateTime.Now);

            pageDetails = GetContent(url);

            if (File.Exists(fileName))
            {
                xdoc = XDocument.Load(fileName);
                xdoc.Root.Add(new XElement("RssItem", new XAttribute("title", pageDetails.Item1)
                , new XAttribute("content", pageDetails.Item2), new XAttribute("URI", url)));
            }
            else
            {
                xdoc = new XDocument();
                xdoc.Add(new XElement("Root"));

                xdoc.Root.Add(new XElement("RssItem", new XAttribute("title", pageDetails.Item1)
                           , new XAttribute("content", pageDetails.Item2), new XAttribute("URI", url)));
            }

            xdoc.Save(fileName);

            Trace.WriteLine(string.Format("Stored: {0} on {1}.", url, fileName));
        }

        /// <summary>
        /// Get the url's title and content description. This uses the HtmlAgility pack to parse the HTML.
        /// As not Jon skeet can parse HTML with Regex.
        /// </summary>
        /// <param name="url">Url to get details from</param>
        /// <returns></returns>
        private Tuple<string, string> GetContent(string url)
        {
            HtmlWeb web = new HtmlWeb();
            string title = string.Empty, content = string.Empty;

            try
            {
                HtmlDocument docx = web.Load(url);

                var titulo = docx.DocumentNode.DescendantsAndSelf().Where(x => x.Name == "title").FirstOrDefault();

                title = titulo != null ? titulo.InnerText : string.Empty;

                var contenido = docx.DocumentNode.DescendantsAndSelf()
                    .Where(x => x.Name == "meta" && x.Attributes.Contains("name")
                        && x.Attributes.Where(y => y.Value == "description").Count() == 1)
                    .FirstOrDefault();

                content = contenido != null ?
                    contenido.Attributes.Where(x => x.Name == "content").FirstOrDefault().Value : string.Empty;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Exception processing {0}.\nEx:{1}.", url, ex));
            }

            return new Tuple<string, string>(title, content);
        }
    }
}
