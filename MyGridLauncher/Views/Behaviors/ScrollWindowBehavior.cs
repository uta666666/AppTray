using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyToolsLauncher.Views.Behaviors {
    public class ScrollWindowBehavior : Behavior<Window> {
        public ICommand ScrollCommand {
            get { return (ICommand)GetValue(ScrollCommandProperty); }
            set { SetValue(ScrollCommandProperty, value); }
        }

        public ICommand MiddleClickCommand {
            get { return (ICommand)GetValue(MiddleClickCommandProperty); }
            set { SetValue(MiddleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ScrollCommandProperty = DependencyProperty.Register(nameof(ScrollCommand), typeof(ICommand), typeof(ScrollWindowBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty MiddleClickCommandProperty = DependencyProperty.Register(nameof(MiddleClickCommand), typeof(ICommand), typeof(ScrollWindowBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
            AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
            AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
        }

        private void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e) {
            ScrollCommand?.Execute(e.Delta < 0);
        }

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.MiddleButton == MouseButtonState.Pressed) {
                MiddleClickCommand?.Execute(null);
            }
        }
    }
}
