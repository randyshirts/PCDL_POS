using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace RocketPos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bool IsSingleInstance;

            Mutex m = new Mutex(true, "RocketPOS", out IsSingleInstance);

            if (!IsSingleInstance)
            {
                // Close Program.
                Environment.Exit(0);
                return;
            }
        }
    }
}
