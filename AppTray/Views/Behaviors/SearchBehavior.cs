using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppTray.Views.Behaviors {
    public class SearchBehavior : Behavior<Window> {
        public string InputControlName {
            get { return (string)GetValue(InputControlNameProperty); }
            set { SetValue(InputControlNameProperty, value); }
        }

        public static readonly DependencyProperty InputControlNameProperty = DependencyProperty.Register(nameof(InputControlName), typeof(string), typeof(SearchBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (IsFunctionKey(e)) {
                return;
            }
            if (IsModified(e)) {
                return;
            }
            var autoCompleteBox = (AutoCompleteBox)AssociatedObject.FindName(InputControlName);
            if (autoCompleteBox != null) {
                var innerTextBox = autoCompleteBox.Template.FindName("Text", autoCompleteBox) as TextBox;
                if (innerTextBox != null) {
                    innerTextBox.Focus();
                }
            }
        }

        private bool IsFunctionKey(KeyEventArgs e) {
            switch (e.Key) {
                case Key.F1:
                case Key.F2:
                case Key.F3:
                case Key.F4:
                case Key.F5:
                case Key.F6:
                case Key.F7:
                case Key.F8:
                case Key.F9:
                case Key.F10:
                case Key.F11:
                case Key.F12:
                    return true;
                case Key.System:
                    if (e.SystemKey == Key.F10) {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private bool IsModified(KeyEventArgs e) {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                return true;
            }
            //if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
            //    return true;
            //}
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) {
                return true;
            }
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows) {
                return true;
            }
            return false;
        }
    }
}
