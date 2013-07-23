namespace Convert
{
    partial class Convert
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
            this.ticker = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.ticker)).BeginInit();
            // 
            // ticker
            // 
            this.ticker.Interval = 60000D;
            this.ticker.Elapsed += new System.Timers.ElapsedEventHandler(this.ticker_Elapsed);
            // 
            // Convert
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.ticker)).EndInit();

        }

        #endregion

        private System.Timers.Timer ticker;

    }
}
