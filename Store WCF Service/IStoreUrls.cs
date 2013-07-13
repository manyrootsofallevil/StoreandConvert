using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace StoreAndConvert.WCFService
{
    [ServiceContract]
    public interface IStoreUrls
    {
        [OperationContract]
         
        string Store(string url, string title);

    }

    //[DataContract]
    //public class Page
    //{
    //    [DataMember]
    //    public string Url { get; set; }

    //    [DataMember]
    //    public string Title { get; set; }

    //    [DataMember]
    //    public string Content { get; set; }

    //    public Page(string url, string title, string content)
    //    {
    //        Url = url;
    //        Title = title;
    //        Content = content;
    //    }

    //}
}
