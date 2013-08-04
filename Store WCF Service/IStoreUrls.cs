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
}
