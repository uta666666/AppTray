using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using AppTray.Models;

namespace AppTray.Views.Behaviors {
    public class KeepAliveWindowBehavior : Behavior<Window> {
        public bool CanCloseWindow {
            get { return (bool)GetValue(CanCloseWindowProperty); }
            set { SetValue(CanCloseWindowProperty, value); }
        }

        public ButtonInfo SavingButtonInfo {
            get { return (ButtonInfo)GetValue(SavingButtonInfoProperty); }
            set { SetValue(SavingButtonInfoProperty, value); }
        }

        public bool IsShowDeactivate {
            get { return (bool)GetValue(IsShowDeactivateProperty); }
            set { SetValue(IsShowDeactivateProperty, value); }
        }

        public static DependencyProperty CanCloseWindowProperty = DependencyProperty.Register(nameof(CanCloseWindow), typeof(bool), typeof(KeepAliveWindowBehavior), new PropertyMetadata(null));

        public static DependencyProperty SavingButtonInfoProperty = DependencyProperty.Register(nameof(SavingButtonInfo), typeof(ButtonInfo), typeof(KeepAliveWindowBehavior), new PropertyMetadata(null));

        public static DependencyProperty IsShowDeactivateProperty = DependencyProperty.Register(nameof(IsShowDeactivate), typeof(bool), typeof(KeepAliveWindowBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.Closing += AssociatedObject_Closing;
            AssociatedObject.Deactivated += AssociatedObject_Deactivated;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.Closing -= AssociatedObject_Closing;
            AssociatedObject.Deactivated -= AssociatedObject_Deactivated;
        }

        private void AssociatedObject_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = !CanCloseWindow;
            AssociatedObject.WindowState = WindowState.Minimized;
            CanCloseWindow = true;

            SaveSettings();
        }

        private void AssociatedObject_Deactivated(object sender, EventArgs e) {
            if (IsShowDeactivate) {
                return;
            }
            AssociatedObject.WindowState = WindowState.Minimized;

            SaveSettings();
        }

        private void SaveSettings() {
            if (SavingButtonInfo == null) {
                return;
            }
            SavingButtonInfo.Save(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }
    }
}
