using AppTray.Commons;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AppTray.Views.Behaviors {
    public class MovePageBehavior : Behavior<Rectangle> {
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        public bool IsNext {
            get { return (bool)GetValue(IsNextProperty); }
            set { SetValue(IsNextProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MovePageBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty IsNextProperty = DependencyProperty.Register(nameof(IsNext), typeof(bool), typeof(MovePageBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.AllowDrop = true;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            AssociatedObject.DragLeave += AssociatedObject_DragLeave;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.AllowDrop = false;
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
        }

        private void AssociatedObject_DragLeave(object sender, DragEventArgs e) {
            if (_timer != null && _timer.IsEnabled) {
                _timer.Stop();
            }
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e) {
            if (!GlobalExclusionInfo.IsDragDroping) {
                return;
            }
            if (_timer == null || !_timer.IsEnabled) {
                _timer = new DispatcherTimer(DispatcherPriority.Normal);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
        }

        private DispatcherTimer _timer;

        private void Timer_Tick(object sender, EventArgs e) {
            if (!GlobalExclusionInfo.IsDragDroping) {
                _timer.Stop();
                return;
            }
            Command?.Execute(IsNext);
        }
    }
}
