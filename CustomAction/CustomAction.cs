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
        [CustomAction]
        public static ActionResult GiveWriteAccess(Session session)
        {
            ActionResult result = ActionResult.Failure;
            // Debugger.Launch();
            session.Log("Begin GiveWriteAccess");

            result = CustomActionHelper.SetAcl(@"NT AUTHORITY\Network Service", session["INSTALLFOLDER"], session) == true
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
        public static ActionResult DropTrailingSlash(Session session)
        {

            session.Log("Begin DropTrailingSlash");

            session["ASPNETREGIIS"] = session["ASPNETREGIIS"].ToString().Trim();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = session["ASPNETREGIIS"],
                    Arguments = " -pa 'NetFrameWorkConfigurationKey' 'NT Authority\\Network Service'",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            session.Log("{0}{1}", session["ASPNETREGIIS"], "-pa 'NetFrameWorkConfigurationKey' 'NT Authority\\Network Service'");

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                session.Log(proc.StandardOutput.ReadLine());
            }
            session.Log("End DropTrailingSlash");

            return ActionResult.Success;
        }

    }

}
