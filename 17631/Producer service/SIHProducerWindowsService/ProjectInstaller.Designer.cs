namespace SIHProducerWindowsService
{
    partial class ProjectInstaller
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
            this.sifmProducerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SIFMProducerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // sifmProducerProcessInstaller
            // 
            this.sifmProducerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.sifmProducerProcessInstaller.Password = null;
            this.sifmProducerProcessInstaller.Username = null;
            // 
            // SIFMProducerInstaller
            // 
            this.SIFMProducerInstaller.Description = "SIFM Producer";
            this.SIFMProducerInstaller.DisplayName = "SIFM Producer";
            this.SIFMProducerInstaller.ServiceName = "SIFMDBProducer";
            this.SIFMProducerInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SIFMProducerInstaller,
            this.sifmProducerProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller sifmProducerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SIFMProducerInstaller;
    }
}