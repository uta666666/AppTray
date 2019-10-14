using AppTray.Commons;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AppTray.Views.Behaviors {
    public class ShortcutBehavior : Behavior<Window> {
        public Dictionary<KeyData, ICommand> ShortcutCommand {
            get { return (Dictionary<KeyData, ICommand>)GetValue(ShortcutCommandProperty); }
            set { SetValue(ShortcutCommandProperty, value); }
        }

        public static readonly DependencyProperty ShortcutCommandProperty = DependencyProperty.Register(nameof(ShortcutCommand), typeof(Dictionary<KeyData, ICommand>), typeof(ShortcutBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (ShortcutCommand == null) {
                return;
            }
            var keyCode = e.Key == Key.System ? e.SystemKey : e.Key;
            var key = ShortcutCommand.Keys.Where(n => n.KeyCode == keyCode && n.Modifier == e.KeyboardDevice.Modifiers);
            if (key.Any()) {
                var command = ShortcutCommand[key.First()];
                command?.Execute(null);
            } else {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(keyCode);
                System.Diagnostics.Debug.WriteLine(e.KeyboardDevice.Modifiers);
#endif
            }

        }
    }
}
