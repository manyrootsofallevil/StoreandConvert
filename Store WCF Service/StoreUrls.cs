using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel.Activation;

namespace StoreAndConvert.WCFService
{
    //Using this bookmarklet. Obviously change lled to your hostname.
    //Bear in mind that an certificate will be
    //javascript:function call(){var e=document,i=e.location;$.ajax({url: i.protocol + '//lled/storeurl/store?page=' + encodeURIComponent(i),async:false,contentType:'application/json',dataType:'jsonp',success:function() {e.title='Saved';},error:function(){alert('Error');} })} call();void(0)
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StoreUrls : IStoreUrls
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    UriTemplate = "store?page={url}&title={title}")]

        public string Store(string url, string title)
        {
            try
            {
                string directory = ConfigurationManager.AppSettings["StoreDirectory"];

                //It would seem like a better idea to make this static 
                WebPage storeMe = new WebPage(directory);
                storeMe.Store(url, title);

                return url;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Exception: {0}.", ex));
                return ex.ToString();
            }
        }

    }
}
