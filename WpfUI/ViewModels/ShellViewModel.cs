using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfUI.Models;

namespace WpfUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        public string ClipText
        {
            get { return _clipText; }
            set
            {
                if (value != _clipText)
                {
                    _clipText = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string _clipText;

        public string Separator
        {
            get { return separator; }
            set
            {
                if (value != separator)
                {
                    separator = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string separator;

        public double Timeout
        {
            get { return _timeout; }
            set
            {
                if (value != _timeout)
                {
                    _timeout = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private double _timeout = 10.0;

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

        public bool CanClearText(string clipText) => !String.IsNullOrWhiteSpace(clipText);

        public void ClearText(string clipText)
        {
            ClipText = null;
        }

        public void Exit()
        {
            allowExit = true;
            TryClose();
        }

        public void SetClipboard()
        {
            Debug.WriteLine("SetClipboard.");

            setText = ClipText;
            lastUpdateTime = DateTime.Now;
            Clipboard.SetText(ClipText);
        }

        public void ClipboardUpdated()
        {
            Debug.WriteLine("ClipboardUpdated.");

            if (Clipboard.ContainsText())
            {
                var text = Clipboard.GetText();
                Debug.WriteLine($"    text = '{text}'.");

                var now = DateTime.Now;

                // If timeout expired replace clip text with current text
                if ((now - lastUpdateTime) > TimeSpan.FromSeconds(Timeout))
                {
                    ClipText = text;
                    lastUpdateTime = now;
                    setText = null;
                }
                // otherwise if this is not a callback from SetClipboard append the text to clip text.
                else if (text != setText)
                {
                    ClipText += (Separator + text);
                    lastUpdateTime = now;
                    setText = null;
                }    
            }
        }

        public override void CanClose(Action<bool> callback)
        {
            if (!allowExit)
            {
                // Notify user
            }

            callback(allowExit); // will cancel close unless allowExit is true
        }

        private DateTime lastUpdateTime = DateTime.MinValue;
        private string setText;
        private bool allowExit;
    }
}
