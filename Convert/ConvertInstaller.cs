using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Convert
{
    [RunInstaller(true)]
    public partial class ConvertInstaller : System.Configuration.Install.Installer
    {
        public ConvertInstaller()
        {
            InitializeComponent();
        }
    }
}
