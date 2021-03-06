﻿using System;
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

                if (!string.IsNullOrEmpty(certSubjectName)) { AddServiceEndPoint(host, "https://{0}:{1}/storeurl", true, certSubjectName); }
                AddServiceEndPoint(host, "http://{0}:{1}/storeurl", false);

                host.Open();

                Trace.TraceInformation("Service host running......");
                Trace.TraceInformation("Listening on");

                foreach (ServiceEndpoint sep in host.Description.Endpoints)
                {
                    Trace.TraceInformation(string.Format("endpoint: {0} - BindingType: {1}",
                        sep.Address, sep.Binding.Name));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("An error occurred during start up. Exception:", ex);
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
                Trace.TraceError("Error Stopping", ex);
            }
        }

        private void AddServiceEndPoint(ServiceHost host, string url, bool useSSLTLS, string certSubjectName = "")
        {
            string addressHttp = string.Empty;

            WebHttpBinding binding;

            if (useSSLTLS)
            {
                binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                binding.HostNameComparisonMode = HostNameComparisonMode.WeakWildcard;
                binding.CrossDomainScriptAccessEnabled = true;

                addressHttp = String.Format(url,
                System.Net.Dns.GetHostEntry("").HostName, ConfigurationManager.AppSettings["SSLPort"]);

            }
            else
            {
                binding = new WebHttpBinding(WebHttpSecurityMode.None);
                binding.CrossDomainScriptAccessEnabled = true;

                addressHttp = String.Format(url,
              System.Net.Dns.GetHostEntry("").HostName, ConfigurationManager.AppSettings["HTTPPort"]);
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
