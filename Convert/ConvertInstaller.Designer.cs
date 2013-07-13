namespace Convert
{
    partial class ConvertInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.convertServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.convertProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // convertServiceInstaller
            // 
            this.convertServiceInstaller.Description = "Convert Stored Urls to Ebook (mobi format)";
            this.convertServiceInstaller.DisplayName = "Convert Urls";
            this.convertServiceInstaller.ServiceName = "Convert Urls";
            // 
            // convertProcessInstaller
            // 
            this.convertProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.convertProcessInstaller.Password = null;
            this.convertProcessInstaller.Username = null;
            // 
            // ConvertInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.convertServiceInstaller,
            this.convertProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceInstaller convertServiceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller convertProcessInstaller;

    }
}