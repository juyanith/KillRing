using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.ViewModels
{
    public class StringItemGroup : PropertyChangedBase
    {
        public DateTime TimeStamp
        {
            get => _timeStamp;
            set
            {
                if (_timeStamp != value)
                {
                    _timeStamp = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private DateTime _timeStamp = DateTime.Now;

        public string Separator
        {
            get => _separator;
            set
            {
                if (_separator != value)
                {
                    _separator = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string _separator;

        public BindableCollection<StringItem> Entries { get; } = new BindableCollection<StringItem>();

        public void Add(string text)
        {
            Entries.Add(text);
            TimeStamp = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Join(Separator, Entries);
        }
    }
}
