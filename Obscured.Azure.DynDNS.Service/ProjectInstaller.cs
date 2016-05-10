using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Obscured.Azure.DynDNS.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void ServiceProcessInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
        }

        private void ServiceProcessInstaller_Commited(object sender, InstallEventArgs e)
        {
            using (var sc = new ServiceController(ServiceInstaller.ServiceName))
            {
                sc.Start();
            }
        }
    }
}
