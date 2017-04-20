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
    public class ExecuteSelectedAppBehavior : Behavior<AutoCompleteBox> {
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ExecuteSelectedAppBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.DropDownClosing += AssociatedObject_DropDownClosing;
            AssociatedObject.DropDownClosed += AssociatedObject_DropDownClosed;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.DropDownClosing -= AssociatedObject_DropDownClosing;
            AssociatedObject.DropDownClosed -= AssociatedObject_DropDownClosed;
        }

        private bool _isEnter;
        private bool _isClosing;
        private bool _isCallingCommand;

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e) {
            _isEnter = e.Key == Key.Enter;

            if (_isEnter && !AssociatedObject.IsDropDownOpen) {
                try {
                    CallCommand();
                } finally {
                    _isClosing = false;
                    _isEnter = false;
                }
            }
        }

        private void AssociatedObject_DropDownClosing(object sender, RoutedPropertyChangingEventArgs<bool> e) {
            _isClosing = true;
        }

        private void AssociatedObject_DropDownClosed(object sender, System.Windows.RoutedPropertyChangedEventArgs<bool> e) {
            if (!_isClosing) {
                return;
            }
            if (!_isEnter) {
                return;
            }
            try {
                CallCommand();
            } finally {
                _isClosing = false;
                _isEnter = false;
            }
        }

        private void CallCommand() {
            if (AssociatedObject.SelectedItem == null) {
                return;
            }
            if (_isCallingCommand) {
                return;
            }
            _isCallingCommand = true;
            try {
                Command?.Execute(AssociatedObject.SelectedItem);
            } finally {
                _isCallingCommand = false;
            }
        }
    }
}
