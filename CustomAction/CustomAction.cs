using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Diagnostics;
using System.IO;

namespace CustomAction
{
    public class CustomActions
    {
        //hard coded just because
        static string user = @"NT AUTHORITY\Network Service";

        [CustomAction]
        public static ActionResult CheckElevated(Session session)
        {
            ActionResult result = ActionResult.Failure;

            session.Log("Begin CheckElevated");

            if (CustomActionHelper.CheckRunAsAdministrator())
            {
                result = ActionResult.Success;
            }
            else
            {
                session.Log("Not running with elevated permissions.STOP");
                session.DoAction("NotElevated");
            }

            session.Log("End CheckElevated");

            return result;
        }

        [CustomAction]
        public static ActionResult GiveWriteAccess(Session session)
        {
            ActionResult result = ActionResult.Failure;
            bool results = false;
            // Debugger.Launch();
            session.Log("Begin GiveWriteAccess");

            results = CustomActionHelper.SetAcl(user, session["TempFolder"], session);
               
            result = results && CustomActionHelper.SetAcl(user, session["INSTALLFOLDER"], session) == true
                ? ActionResult.Success : ActionResult.Failure;

            session.Log("End GiveWriteAccess");

            return result;
        }

        [CustomAction]
        public static ActionResult RemoveWriteAccess(Session session)
        {
            ActionResult result = ActionResult.Failure;
            bool results = false;
            // Debugger.Launch();
            session.Log("Begin GiveWriteAccess");

            results = CustomActionHelper.RemoveAcl(user, session["TempFolder"], session);

            result = results && CustomActionHelper.RemoveAcl(user, session["INSTALLFOLDER"], session) == true
                ? ActionResult.Success : ActionResult.Failure;

            session.Log("End GiveWriteAccess");

            return result;
        }

        [CustomAction]
        public static ActionResult EncryptConfigFile(Session session)
        {
            ActionResult result = ActionResult.Failure;
            // Debugger.Launch();
            session.Log("Begin EncryptConfigFile");

            result = CustomActionHelper.EncryptAppSettings(Path.Combine(session["INSTALLFOLDER"], "Convert"), "Convert.exe") == true
                ? ActionResult.Success : ActionResult.Failure;

            session.Log("End EncryptConfigFile");

            return result;
        }

        [CustomAction]
        public static ActionResult GetInstalledCertificates(Session session)
        {
            ActionResult result = ActionResult.Failure;

            session.Log("Start GetInstalledCertificates");

            //Debugger.Launch();

            View cView = session.Database.OpenView("select * from ComboBox");
            cView.Execute();

            CustomActionHelper.PopulateDropDownList(session, cView);

            cView.Close();

            session.Log("End GetInstalledCertificates");

            result = ActionResult.Success;

            return result;


        }

        [CustomAction]
        public static ActionResult SetCertThumprint(Session session)
        {
            ActionResult result = ActionResult.Failure;

            session.Log("Start SetCertThumprint");

            session["CERTTHUMBPRINT"] = CustomActionHelper.GetCertificateThumbprint(session["CERT"], session);

            result = ActionResult.Success;

            session.Log("End SetCertThumprint");

            return result;
        }

        [CustomAction]
        public static ActionResult NetSH(Session session)
        {//I realize that this is suboptimal but could not get the removal working through standard wix custom actions.
            ActionResult result = ActionResult.Failure;

            session.Log("Start SH");

            session.Log("{0}", CustomActionHelper.StartProcess(string.Format("http del sslcert ipport=0.0.0.0:{0}", session["SSLPORT"])));
            session.Log("{0}", CustomActionHelper.StartProcess(string.Format("http del urlacl url=https://*:{0}/Storeurl", session["SSLPORT"])));
            session.Log("{0}", CustomActionHelper.StartProcess(string.Format("http del urlacl url=http://+:{0}/Storeurl", session["HTTPPORT"])));
            
            result = ActionResult.Success;

            session.Log("End SH");

            return result;

        }

    }

}
