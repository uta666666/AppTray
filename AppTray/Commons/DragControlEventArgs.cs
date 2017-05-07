using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppTray.Commons {
    public class DragControlEventArgs : RoutedEventArgs {
        public DragControlEventArgs(DragEventArgs e, int toPageNo, int toButtonNo) {
            DragEvent = e;
            var data = e.Data.GetData(typeof(string)).ToString().Split(',');
            FromPageNo = int.Parse(data[0].ToString());
            FromButtonNo = int.Parse(data[1].ToString());
            ToPageNo = toPageNo;
            ToButtonNo = toButtonNo;
        }

        public DragEventArgs DragEvent { get; set; }

        public int FromPageNo { get; set; }

        public int FromButtonNo { get; set; }

        public int ToPageNo { get; set; }

        public int ToButtonNo { get; set; }
    }
}
