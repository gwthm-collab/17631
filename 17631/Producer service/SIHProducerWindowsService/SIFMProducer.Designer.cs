namespace SIHProducerWindowsService
{
    partial class SIFMDBProducer
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
            this.SIFMLogger = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.SIFMLogger)).BeginInit();
            // 
            // SIFMLogger
            // 
            this.SIFMLogger.Log = "Application";
            this.SIFMLogger.Source = "SIFM Producer";
            // 
            // SIFMDBProducer
            // 
            this.ServiceName = "SIFMDBProducer";
            ((System.ComponentModel.ISupportInitialize)(this.SIFMLogger)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog SIFMLogger;
    }
}
