using AppTray.Commons;
using AppTray.Models;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AppTray.Views.Behaviors {
    public class HotKeyBehavior : Behavior<Window> {
        public HotKey HotKey {
            get { return (HotKey)GetValue(HotKeyProperty); }
            set { SetValue(HotKeyProperty, value); }
        }

        public static readonly DependencyProperty HotKeyProperty = DependencyProperty.Register(nameof(HotKey), typeof(HotKey), typeof(HotKeyBehavior), new PropertyMetadata(new HotKey() { Key = Key.None, Modifiers = ModifierKeys.None }, HotKeyChanged));

        private static HotKeyHelper _hotkey;

        private static void HotKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (_hotkey == null) {
                return;
            }
            var obj = d as HotKeyBehavior;
            _hotkey.UnregisterAll();
            _hotkey.Register(
                obj.HotKey.Modifiers,
                obj.HotKey.Key,
                (_, __) => {
                    if (obj.AssociatedObject.WindowState == WindowState.Normal) {
                        //AssociatedObject.Activate();
                    } else {
                        obj.AssociatedObject.WindowState = WindowState.Normal;
                        obj.AssociatedObject.Activate();
                    }
                });
        }

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Closed += AssociatedObject_Closed;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Closed -= AssociatedObject_Closed;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
            _hotkey = new HotKeyHelper(AssociatedObject);
            _hotkey.Register(
                HotKey.Modifiers,
                HotKey.Key,
                (_, __) => {
                    if (AssociatedObject.WindowState == WindowState.Normal) {
                        //AssociatedObject.Activate();
                    } else {
                        AssociatedObject.WindowState = WindowState.Normal;
                        AssociatedObject.Activate();
                    }
                });
        }

        private void AssociatedObject_Closed(object sender, EventArgs e) {
            _hotkey.Dispose();
        }
    }
}
