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
            Groups.CollectionChanged += Groups_CollectionChanged;
        }
        
        public int MaxGroups
        {
            get { return _maxGroups; }
            set
            {
                if (value < 1) value = 1;
                if (value != _maxGroups)
                {
                    _maxGroups = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private int _maxGroups = 9;

        public int MaxTextLength
        {
            get { return _maxTextLength; }
            set
            {
                if (value < 1) value = 1;
                if (value != _maxTextLength)
                {
                    _maxTextLength = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private int _maxTextLength = 524288; // 1 MB (512 * 1024 * 2 bytes per char)

        public string Separator
        {
            get { return _separator; }
            set
            {
                if (value != _separator)
                {
                    _separator = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string _separator;

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
        private double _timeout = 4.0;

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

        public BindableCollection<StringItemGroup> Groups { get; } = new BindableCollection<StringItemGroup>();

        public StringItemGroup SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (!ReferenceEquals(value, _selectedGroup))
                {
                    _selectedGroup = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private StringItemGroup _selectedGroup;

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
            Groups.Clear();
        }

        public void Exit()
        {
            allowExit = true;
            TryClose();
        }

        public void SetClipboard()
        {
            if (SelectedGroup != null)
            {
                // Keep a reference to the data object because we will get a clipboard update when it is sent to the keyboard.
                setData = new DataObject(DataFormats.Text, SelectedGroup.ToString(), true);
                Clipboard.SetDataObject(setData);
            }
        }

        public void TrimEntries()
        {
            foreach (var entry in SelectedGroup?.Entries)
            {
                entry.Text = entry.Text.Trim();
            }
        }

        public void ClipboardUpdated()
        {
            // Only process text but not text we have set.
            if ((setData == null || !Clipboard.IsCurrent(setData)) && Clipboard.ContainsText())
            {
                setData = null; // No need to keep reference (if any).

                var text = Clipboard.GetText();
                if (text.Length <= MaxTextLength)
                {
                    // Associate this entry with previous unless timeout has expired.
                    var group = Groups.LastOrDefault();
                    if (group == null || (DateTime.Now - group.TimeStamp) > TimeSpan.FromSeconds(Timeout))
                    {
                        group = new StringItemGroup();
                        Groups.Add(group);
                        SelectedGroup = group;
                        if (Groups.Count > MaxGroups)
                        {
                            Groups.RemoveAt(0);
                        }
                    }
                    group.Add(text);
                }
            }
        }

        public override void CanClose(Action<bool> callback)
        {
            if (!allowExit)
            {
                var result = MessageBox.Show("Are you sure? This will turn off Kill Ring.\n\nClick 'OK' to exit application.", "Confirm Exit", MessageBoxButton.OKCancel);
                allowExit = result == MessageBoxResult.OK;
            }

            callback(allowExit); // Will cancel close unless allowExit is true.
        }

        private void Groups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanClearEntries = Groups.Count > 0;
        }

        private bool allowExit;
        private IDataObject setData;
    }
}
