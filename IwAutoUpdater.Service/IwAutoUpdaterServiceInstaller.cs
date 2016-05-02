using System.ComponentModel;

namespace SuperVisor.Service
{
    [RunInstaller(true)]
    public partial class IwAutoUpdaterServiceInstaller : System.Configuration.Install.Installer
    {
        public IwAutoUpdaterServiceInstaller()
        {
            InitializeComponent();
        }
    }
}