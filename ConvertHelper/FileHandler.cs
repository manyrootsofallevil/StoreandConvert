using HostFeed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertHelper
{
    public static class FileHandler
    {
        public static List<HTMLPage> GetFiles(string path)
        {
            List<HTMLPage> webPages = new List<HTMLPage>();

            try
            {
                string[] files = Directory.GetFiles(path, "*xml");

                foreach (string file in files)
                {

                    XDocument xdoc = XDocument.Load(file);

                    foreach (var item in xdoc.Root.Descendants())
                    {

                        webPages.Add(
                            new HTMLPage(
                                new Tuple<string, string, string>(
                                    item.Attribute("URI").Value, item.Attribute("content").Value,
                                    item.Attribute("title").Value)));
                    }


                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("An error occurred on GetFiles(). Path: {0} Exception: {1}.", path, ex));             
            }

            return webPages;
        }

        public static void MoveFiles(string sourcePath, string destPath)
        {
            string currentFile = string.Empty;

            try
            {
                string[] files = Directory.GetFiles(sourcePath, "*xml");

                foreach (string file in files)
                {
                    currentFile = file;
                    File.Move(file, string.Format("{0}{1}_processed{2}", 
                        destPath, Path.GetFileNameWithoutExtension(file), Path.GetExtension(file)));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("An error occurred while moving file {0}. Exception: {1}.", currentFile, ex));             
            }

             
        }
    }
}
