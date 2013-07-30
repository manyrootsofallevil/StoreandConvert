using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertHelper
{
    public static class EncryptAppConfig
    {
        public static void EncryptAppSettings()
        {

            Trace.TraceInformation("Encrypting App Settings");


            // Get the current configuration file.
            Configuration config =
                    ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetCallingAssembly().Location);

            ConfigurationSection section = config.GetSection("appSettings");

            if (!section.SectionInformation.IsProtected)
            {
                // Protect (encrypt)the section.
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");

                // Save the encrypted section.
                section.SectionInformation.ForceSave = true;

                try
                {
                    config.Save(ConfigurationSaveMode.Full);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("An error ocurred. Exception: {0}", ex);
                }

                ConfigurationManager.RefreshSection("configuration");
            }
            else
            {
                Trace.TraceInformation("App Settings already encrypted");
            }
        }
    }
}
