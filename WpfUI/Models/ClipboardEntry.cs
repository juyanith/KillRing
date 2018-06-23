using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUI.Models
{
    public class ClipboardEntry
    {
        public ClipboardEntry()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; }
        public ClipboardEntryGroup Group { get; set; }
        public String Text { get; set; }
    }
}
