using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.Models
{
    public class ClipboardEntryGroup : List<ClipboardEntry>
    {
        public string Separator { get; set; }
    }
}
