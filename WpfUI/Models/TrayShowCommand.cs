using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfUI.Models
{
    public class TrayShowCommand : ICommand
    {
#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is Window window)
            {
                if (window.Visibility == Visibility.Visible)
                {
                    window.Hide();
                }
                else
                {
                    window.Show();
                    window.Activate();
                }
            }
        }
    }
}
