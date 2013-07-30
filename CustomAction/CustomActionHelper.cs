﻿using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CustomAction
{
    internal static class CustomActionHelper
    {
        internal static bool SetAcl(string user, string destinationDirectory, Session session)
        {
            session.Log("Begin SetACL. User: {0}. Directory: {1}", user, destinationDirectory);

            if (!Directory.Exists(destinationDirectory))
            {
                session.Log("Creating Directory {0}", destinationDirectory);
                Directory.CreateDirectory(destinationDirectory);
            }
        
            FileSystemRights Rights = (FileSystemRights)0;
            Rights = FileSystemRights.FullControl;

            // *** Add Access Rule to the actual directory itself
            FileSystemAccessRule AccessRule = new FileSystemAccessRule(user, Rights,
                                        InheritanceFlags.None,
                                        PropagationFlags.NoPropagateInherit,
                                        AccessControlType.Allow);

            DirectoryInfo Info = new DirectoryInfo(destinationDirectory);
            DirectorySecurity Security = Info.GetAccessControl(AccessControlSections.Access);

            bool Result = false;
            Security.ModifyAccessRule(AccessControlModification.Set, AccessRule, out Result);

            if (!Result)
            {
                return false;
            }

            // *** Always allow objects to inherit on a directory
            InheritanceFlags iFlags = InheritanceFlags.ObjectInherit;
            iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

            // *** Add Access rule for the inheritance
            AccessRule = new FileSystemAccessRule(user, Rights,
                                        iFlags,
                                        PropagationFlags.InheritOnly,
                                        AccessControlType.Allow);
            Result = false;
            Security.ModifyAccessRule(AccessControlModification.Add, AccessRule, out Result);

            if (!Result)
            {
                return false;
            }

            Info.SetAccessControl(Security);
            session.Log("End SetACL.");
            return true;
        }

        internal static bool EncryptAppSettings(string destinationDirectory, string exeName)
        {

            Trace.TraceInformation("Encrypting App Settings");
            // Get the current configuration file.
            Configuration config =
                    ConfigurationManager.OpenExeConfiguration(Path.Combine(destinationDirectory,exeName));

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
                    return false;
                }

                ConfigurationManager.RefreshSection("configuration");
            }
            else
            {
                Trace.TraceInformation("App Settings already encrypted");
            }

            return true;
        }

        internal static void PopulateDropDownList(Session session, View cView)
        {
            X509Store store = new X509Store("My", StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadOnly);

            int i = 1;

            session.Log("Enumerate Certificates");

            foreach (X509Certificate2 cert in store.Certificates)
            {
                session.Log(string.Format("Processing {0}", cert.SubjectName.Name));

                Record record = new Record(3);

                string friendlyName = string.IsNullOrEmpty(cert.FriendlyName) ? cert.SubjectName.Name : cert.FriendlyName;

                record.SetString(1, "CERT");
                record.SetInteger(2, i);
                record.SetString(3, cert.SubjectName.Name.Split('=').Skip(1).Take(1).FirstOrDefault());

                session.Log(string.Format("Adding Record {0} {1}.", i, friendlyName));

                cView.Modify(ViewModifyMode.InsertTemporary, record);

                i++;
            }
        }
    }
}
