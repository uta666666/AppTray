using MyToolsLauncher.Commons;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyToolsLauncher.Views.Behaviors {
    public class DragStartBehavior : Behavior<FrameworkElement> {
        private Point _origin;
        private bool _isButtonDown;
        private DragGhost _dragGhost;

        public DragDropEffects AllowedEffects {
            get { return (DragDropEffects)GetValue(AllowedEffectsProperty); }
            set { SetValue(AllowedEffectsProperty, value); }
        }

        public static readonly DependencyProperty AllowedEffectsProperty = DependencyProperty.Register("AllowedEffects", typeof(DragDropEffects), typeof(DragStartBehavior), new UIPropertyMetadata(DragDropEffects.All));

        public object DragDropData {
            get { return GetValue(DragDropDataProperty); }
            set { SetValue(DragDropDataProperty, value); }
        }

        public static readonly DependencyProperty DragDropDataProperty = DependencyProperty.Register("DragDropData", typeof(object), typeof(DragStartBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
            AssociatedObject.QueryContinueDrag += AssociatedObject_QueryContinueDrag;
        }

        protected override void OnDetaching() {
            AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
            AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;
            AssociatedObject.QueryContinueDrag -= AssociatedObject_QueryContinueDrag;
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            _origin = e.GetPosition(AssociatedObject);
            _isButtonDown = true;
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed || !_isButtonDown) {
                return;
            }
            var point = e.GetPosition(AssociatedObject);
            if (CheckDistance(point, _origin)) {
                GlobalExclusionInfo.IsDragDroping = true;
                try {
                    if (_dragGhost == null) {
                        _dragGhost = new DragGhost(sender as UIElement, 0.5, e.GetPosition(sender as UIElement));
                        _dragGhost.Show();
                    }
                    DragDrop.DoDragDrop(AssociatedObject, DragDropData, AllowedEffects);
                } finally {
                    GlobalExclusionInfo.IsDragDroping = false;
                }
                _isButtonDown = false;
                e.Handled = true;

                if (_dragGhost != null) {
                    _dragGhost.Remove();
                    _dragGhost = null;
                }
            }
        }

        private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
            _isButtonDown = false;
        }

        private void AssociatedObject_QueryContinueDrag(object sender, QueryContinueDragEventArgs e) {
            if (_dragGhost != null) {
                // ゴーストの位置を更新
                _dragGhost.Position = CursorInfo.GetNowPosition(AssociatedObject);
            }
        }

        private bool CheckDistance(Point x, Point y) {
            return Math.Abs(x.X - y.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                   Math.Abs(x.Y - y.Y) >= SystemParameters.MinimumVerticalDragDistance;
        }
    }
}
