using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;

namespace StoreAndConvert.WCFService
{
    //Using this bookmarklet. For some reason, that i'm yet to find out, the ajax call triggers the error even though the call succeeds, 
    //i.e. there is a 200 response on Fiddler and the url is stored.  
    //javascript:function call(){function e(){console.log("called test")}var t=document,n=t.location;$.ajax({url:n.protocol+"//lled/storeurl/store?page="+encodeURIComponent(n),async:false,contentType:"application/json",dataType:"jsonp",jsonpCallback:"test",success:function(e){t.title="Saved"},error:function(e){if(typeof e!="undefined"){t.title="Saved"}if(e.StatusCode=="undefined"){alert("error: "+e)}}})}if(!($=window.jQuery)){script=document.createElement("script");script.src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js";script.onload=call;document.body.appendChild(script)}else{call()}
    public class StoreUrls : IStoreUrls
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "store?page={url}")]
        public string Store(string url)
        {
            string directory = ConfigurationManager.AppSettings["StoreDirectory"];

            //It would seem like a better idea to make this static 
            WebPage storeMe = new WebPage(directory);
            storeMe.Store(url);

            return url;
        }

    }
}
