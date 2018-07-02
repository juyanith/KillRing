using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfUI.ViewModels;

namespace WpfUI
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            string mutexName = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
            using (Mutex mutex = new Mutex(true, mutexName, out bool createdNew))
            {
                if (createdNew)
                {
                    DisplayRootViewFor<ShellViewModel>(new Dictionary<string, object>
                    {
                        { "ShowInTaskbar", false },
                        { "Visibility", Visibility.Hidden },
                    });
                }
                else // Only allow one copy of application to run
                {
                    DisplayRootViewFor<DuplicateViewModel>(); 
                }
            }
        }
    }
}
