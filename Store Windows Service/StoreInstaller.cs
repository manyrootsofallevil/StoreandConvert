using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Store
{
    [RunInstaller(true)]
    public partial class StoreInstaller : System.Configuration.Install.Installer
    {
        public StoreInstaller()
        {
            InitializeComponent();
        }
    }
}
