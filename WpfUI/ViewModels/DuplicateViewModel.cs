using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.ViewModels
{
    public class DuplicateViewModel : Screen
    {
        public string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                if (value != _viewTitle)
                {
                    _viewTitle = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string _viewTitle = "Kill Ring";

        

        public string ExplanationText
        {
            get { return _explanationText; }
            set
            {
                if (value != _viewTitle)
                {
                    _explanationText = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string _explanationText = "Another version of Kill Ring detected and only one is allowed to run at a time. Click Exit or close this window to exit the application.";


        public void Exit()
        {
            TryClose();
        }
    }
}
