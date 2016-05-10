using System.Configuration.Install;
using System.Security.AccessControl;
using System.ServiceProcess;

namespace Obscured.Azure.DynDNS.Service
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
            this.ServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServiceProcessInstaller
            // 
            this.ServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ServiceProcessInstaller.Password = null;
            this.ServiceProcessInstaller.Username = null;
            this.ServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceProcessInstaller_AfterInstall);
            this.ServiceProcessInstaller.Committed += new System.Configuration.Install.InstallEventHandler(this.ServiceProcessInstaller_Commited);
            // 
            // ServiceInstaller
            // 
            this.ServiceInstaller.Description = "Updates a record with the exernal ip-address of the computer running the service," +
    " each hour the ip-address is checked by selected source.";
            this.ServiceInstaller.DisplayName = "Obscured.Azure.DynDNS";
            this.ServiceInstaller.ServiceName = "Obscured.Azure.DynDNS";
            this.ServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServiceProcessInstaller,
            this.ServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ServiceInstaller;
    }
}