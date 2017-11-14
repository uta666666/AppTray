using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AppTray.Views.Behaviors {
    public class KeyToTextBehavior : Behavior<FrameworkElement> {

        public Key Key {
            get { return (Key)GetValue(KeyTextProperty); }
            set { SetValue(KeyTextProperty, value); }
        }

        public static readonly DependencyProperty KeyTextProperty = DependencyProperty.Register(nameof(Key), typeof(Key), typeof(KeyToTextBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            //AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            //AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e) {
            Key = e.Key;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e) {
            Key = e.Key;
        }
    }
}
