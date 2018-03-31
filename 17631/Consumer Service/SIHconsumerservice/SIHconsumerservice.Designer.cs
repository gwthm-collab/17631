namespace SIHconsumerservice
{
    partial class SIHconsumerservice
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
            this.components = new System.ComponentModel.Container();
            this.gospedLog = new System.Diagnostics.EventLog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gospedLog)).BeginInit();
            // 
            // gospedLog
            // 
            this.gospedLog.Log = "Application";
            this.gospedLog.Source = "GoSPED Logger";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            // 
            // SIHconsumerservice
            // 
            this.ServiceName = "SIHconsumerservice";
            ((System.ComponentModel.ISupportInitialize)(this.gospedLog)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog gospedLog;
        private System.Windows.Forms.Timer timer1;
    }
}
