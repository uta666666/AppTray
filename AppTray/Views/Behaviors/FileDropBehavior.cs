using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppTray.Commons;
using Microsoft.Xaml.Behaviors;

namespace AppTray.Views.Behaviors {
    public class FileDropBehavior : Behavior<Button> {
        public ICommand FileDropCommand {
            get { return (ICommand)GetValue(FileDropCommandProperty); }
            set { SetValue(FileDropCommandProperty, value); }
        }

        public int ButtonNo {
            get { return (int)GetValue(ButtonNoProperty); }
            set { SetValue(ButtonNoProperty, value); }
        }

        public static DependencyProperty FileDropCommandProperty = DependencyProperty.Register("FileDropCommand", typeof(ICommand), typeof(FileDropBehavior), new PropertyMetadata(null));

        public static DependencyProperty ButtonNoProperty = DependencyProperty.Register(nameof(ButtonNo), typeof(int), typeof(FileDropBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;

            AssociatedObject.DragOver += AssociatedObject_DragOver;
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.AllowDrop = false;

            AssociatedObject.DragOver += AssociatedObject_DragOver;
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
                if (files.Count() != 1) {
                    e.Effects = DragDropEffects.None;
                } else {
                    e.Effects = DragDropEffects.Copy;
                }
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e) {
            var files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
            if (files != null && files.Length == 1) {
                FileDropCommand?.Execute(new FileDropParameter() { ButtonNo = this.ButtonNo, FilePath = files[0] });
                e.Handled = true;
            }
        }
    }
}
