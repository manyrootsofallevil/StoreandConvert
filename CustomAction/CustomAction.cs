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
        public static ActionResult GiveWriteAccess(Session session)
        {
            ActionResult result = ActionResult.Failure;
            bool results = false;
            // Debugger.Launch();
            session.Log("Begin GiveWriteAccess");

            results = CustomActionHelper.SetAcl(user, Path.Combine(session["AppDataFolder"], "WebPages"), session);

            result = results && CustomActionHelper.SetAcl(user, session["INSTALLFOLDER"], session) == true
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


    }

}
