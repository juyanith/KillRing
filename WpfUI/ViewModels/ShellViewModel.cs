using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfUI.Models;

namespace WpfUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel()
        {
            ClipboardEntries.CollectionChanged += ClipboardEntries_CollectionChanged;
        }

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

        public BindableCollection<ClipboardEntry> ClipboardEntries { get; } = new BindableCollection<ClipboardEntry>();

        public ClipboardEntry SelectedClipboardEntry
        {
            get { return _selectedClipboardEntry; }
            set
            {
                if (!ReferenceEquals(value, _selectedClipboardEntry))
                {
                    _selectedClipboardEntry = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private ClipboardEntry _selectedClipboardEntry;

        public bool CanClearEntries
        {
            get { return _canClearEntries; }
            private set
            {
                if (value != _canClearEntries)
                {
                    _canClearEntries = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private bool _canClearEntries;

        public void ClearEntries()
        {
            ClipboardEntries.Clear();
        }

        public void Exit()
        {
            allowExit = true;
            TryClose();
        }

        public void SetClipboard()
        {
            Debug.WriteLine("SetClipboard.");


            if (SelectedClipboardEntry != null)
            {
                foreach (var entry in SelectedClipboardEntry.Group)
                {
                    Debug.WriteLine("    SetDataObject.");
                    setData = entry.Data;
                    //Clipboard.SetDataObject(entry, true);
                    NativeMethods.SendCtrlV();
                }
            }
        }

        public void ClipboardUpdated()
        {
            Debug.WriteLine("ClipboardUpdated.");

            if (setData == null || !Clipboard.IsCurrent(setData))
            {
                Debug.WriteLine("    New DataObject.");

                var entry = new ClipboardEntry
                {
                    Data = Clipboard.GetDataObject(),
                };

                var prevEntry = ClipboardEntries.LastOrDefault();
                if (prevEntry == null || (DateTime.Now - prevEntry.TimeStamp) > TimeSpan.FromSeconds(Timeout))
                {
                    entry.Group = new ClipboardEntryGroup();
                }
                else
                {
                    entry.Group = prevEntry.Group;
                }
                entry.Group.Add(entry);

                ClipboardEntries.Add(entry);
                SelectedClipboardEntry = entry;
            }
        }

        public override void CanClose(Action<bool> callback)
        {
            if (!allowExit)
            {
                var result = MessageBox.Show("Are you sure? This will turn off Kill Ring.\n\nClick 'OK' to exit application.", "Confirm Exit", MessageBoxButton.OKCancel);
                allowExit = result == MessageBoxResult.OK;
            }

            callback(allowExit); // will cancel close unless allowExit is true
        }

        private void ClipboardEntries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanClearEntries = ClipboardEntries.Count > 0;
        }

        private bool allowExit;
        private IDataObject setData;
    }
}
