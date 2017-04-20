using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppTray.Commons {
    public class DragControlEventArgs : RoutedEventArgs {
        public DragControlEventArgs(DragEventArgs e, int toButtonNo) {
            DragEvent = e;
            FromButtonNo = int.Parse(e.Data.GetData(typeof(string)).ToString());
            ToButtonNo = toButtonNo;
        }

        public DragEventArgs DragEvent { get; set; }

        public int FromButtonNo { get; set; }

        public int ToButtonNo { get; set; }
    }
}
