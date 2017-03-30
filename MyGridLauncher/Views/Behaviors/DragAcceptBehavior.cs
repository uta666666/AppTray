using MyToolsLauncher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using MyToolsLauncher.Views;
using System.Windows.Controls;
using MyToolsLauncher.Commons;

namespace MyToolsLauncher.Views.Behaviors {
    public sealed class DragAcceptBehavior : Behavior<Grid> {
        public DragAcceptDescription Description {
            get { return (DragAcceptDescription)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(DragAcceptDescription), typeof(DragAcceptBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            AssociatedObject.PreviewDragOver += AssociatedObject_DragOver;
            AssociatedObject.PreviewDrop += AssociatedObject_Drop;
            base.OnAttached();
        }

        protected override void OnDetaching() {
            AssociatedObject.PreviewDragOver -= AssociatedObject_DragOver;
            AssociatedObject.PreviewDrop -= AssociatedObject_Drop;
            base.OnDetaching();
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.None;
                return;
            }
            var desc = Description;
            if (desc == null) {
                e.Effects = DragDropEffects.None;
                return;
            }
            desc.OnDragOver(e);
            e.Handled = true;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e) {
            if (!GlobalExclusionInfo.IsDragDroping) {
                e.Effects = DragDropEffects.None;
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.None;
                return;
            }
            var desc = Description;
            if (desc == null) {
                e.Effects = DragDropEffects.None;
                return;
            }
            var p = e.GetPosition(AssociatedObject);
            int no = 1;
            double x = 0;
            double y = 0;
            foreach (var rowDef in AssociatedObject.RowDefinitions) {
                foreach (var colDef in AssociatedObject.ColumnDefinitions) {
                    Rect r = new Rect(x, y, colDef.ActualWidth, rowDef.ActualHeight);
                    if (r.Contains(p)) {
                        desc.OnDrop(new DragControlEventArgs(e, no));
                        e.Handled = true;
                        return;
                    }
                    x += colDef.ActualWidth;
                    no++;
                }
                x = 0;
                y += rowDef.ActualHeight;
            }
        }
    }
}
