namespace Store
{
    partial class StoreInstaller
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
            this.storeServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.storeServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // storeServiceProcessInstaller
            // 
            this.storeServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.storeServiceProcessInstaller.Password = null;
            this.storeServiceProcessInstaller.Username = null;
            // 
            // storeServiceInstaller
            // 
            this.storeServiceInstaller.Description = "Store Webpages (urls) for later conversion.";
            this.storeServiceInstaller.DisplayName = "Store Urls";
            this.storeServiceInstaller.ServiceName = "Store Urls";
            this.storeServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.storeServiceProcessInstaller,
            this.storeServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller storeServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller storeServiceInstaller;
    }
}