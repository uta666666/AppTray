using AppTray.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AppTray.Views.Behaviors {
    public class ChangePageBehavior : Behavior<Rectangle> {
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ChangePageBehavior), new PropertyMetadata(null));

        public bool IsNext {
            get { return (bool)GetValue(IsNextProperty); }
            set { SetValue(IsNextProperty, value); }
        }

        public static readonly DependencyProperty IsNextProperty = DependencyProperty.Register(nameof(IsNext), typeof(bool), typeof(ChangePageBehavior), new PropertyMetadata(null));

        public int ZIndex {
            get { return (int)GetValue(ZIndexProperty); }
            set { SetValue(ZIndexProperty, value); }
        }

        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.Register(nameof(ZIndex), typeof(int), typeof(ChangePageBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.AllowDrop = true;

            AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;

            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e) {
            if (GlobalExclusionInfo.IsDragDroping) {
                _timer = new DispatcherTimer(DispatcherPriority.Normal);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
        }

        private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
            
        }
        
        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e) {
           
        }

        private DispatcherTimer _timer;
        private void AssociatedObject_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            if (GlobalExclusionInfo.IsDragDroping) {
                _timer = new DispatcherTimer(DispatcherPriority.Normal);
                _timer.Interval = new TimeSpan(0, 0, 1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e) {
            _timer.Stop();
            
            Command?.Execute(IsNext);
            ZIndex = -1;
        }
    }
}
