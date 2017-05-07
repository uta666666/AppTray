using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTray.Commons {
    public class SystemMenuItem {
        public uint MenuID { get; set; }

        public string MenuName { get; set; }

        public ICommand Command { get; set; }
    }
}
