using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using StoreAndConvert.WCFService;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.Web.Administration;


namespace StoreAndConvert.WindowsService
{
    public partial class Store : ServiceBase
    {

        string certSubjectName = string.Empty;

        ServiceHost host;

        public Store()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            try
            {
               //Debugger.Launch();
                certSubjectName = ConfigurationManager.AppSettings["CertificateSubjectName"];

                host = new ServiceHost(typeof(StoreUrls));

                AddServiceEndPoint(host, "https://{0}:3141/storeurl", true, certSubjectName);
                AddServiceEndPoint(host, "http://{0}:2718/storeurl", false);

                host.Open();

                Trace.WriteLine("Service host running......");
                Trace.WriteLine("Listening on");

                foreach (ServiceEndpoint sep in host.Description.Endpoints)
                {
                    Trace.WriteLine(string.Format("endpoint: {0} - BindingType: {1}",
                        sep.Address, sep.Binding.Name));
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("An error occurred during start up. Exception:",ex));
                throw;
            }

        }

        protected override void OnStop()
        {
            try
            {
                if (host != null)
                {
                    host.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void AddServiceEndPoint(ServiceHost host, string url, bool useSSLTLS, string certSubjectName = "")
        {
            string addressHttp = String.Format(url,
                System.Net.Dns.GetHostEntry("").HostName);

            WebHttpBinding binding;

            if (useSSLTLS)
            {
                binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                binding.HostNameComparisonMode = HostNameComparisonMode.WeakWildcard;
                binding.CrossDomainScriptAccessEnabled = true;
            }
            else
            {
                binding = new WebHttpBinding(WebHttpSecurityMode.None);
                binding.CrossDomainScriptAccessEnabled = true;
            }

            // You must create an array of URI objects to have a base address.
            Uri uri = new Uri(addressHttp);
            Uri[] baseAddresses = new Uri[] { uri };

            WebHttpBehavior behaviour = new WebHttpBehavior();
            // Add an endpoint to the service. Insert the thumbprint of an X.509 
            // certificate found on your computer. 
            host.AddServiceEndpoint(typeof(IStoreUrls), binding, uri).EndpointBehaviors.Add(behaviour);

            if (useSSLTLS)
            {
                host.Credentials.ServiceCertificate.SetCertificate(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    X509FindType.FindBySubjectName,
                    certSubjectName);
            }
        }

        private bool EnsureSSLBinding()
        {
            bool result = false;

            using (ServerManager serverManager = new ServerManager())
            {
                //serverManager
            }

            return result;
        }
    }
}
