using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.ViewModels
{
    /// <summary>
    /// Simple wrapper for strings so they can be edited in a ListBox
    /// </summary>
    public class StringItem : PropertyChangedBase
    {
        public static explicit operator string(StringItem stringItem) => stringItem.Text;
        public static implicit operator StringItem(string s) => new StringItem { Text = s };

        public string Text
        {
            get => _text;
            set
            {
                if (value == null) value = "";
                if (_text != value)
                {
                    _text = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        private string _text = "";

        public override string ToString()
        {
            return _text;
        }
    }
}
