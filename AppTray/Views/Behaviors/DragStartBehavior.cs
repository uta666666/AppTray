using AppTray.Commons;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppTray.Views.Behaviors {
    public class DragStartBehavior : Behavior<FrameworkElement> {
        private Point _origin;
        private bool _isButtonDown;
        private DragGhost _dragGhost;

        public DragDropEffects AllowedEffects {
            get { return (DragDropEffects)GetValue(AllowedEffectsProperty); }
            set { SetValue(AllowedEffectsProperty, value); }
        }

        public int CurrentPageNo {
            get { return (int)GetValue(CurrentPageNoProperty); }
            set { SetValue(CurrentPageNoProperty, value); }
        }

        public object DragDropData {
            get { return GetValue(DragDropDataProperty); }
            set { SetValue(DragDropDataProperty, value); }
        }
        
        public int MovePagePanelZIndex {
            get { return (int)GetValue(MovePagePanelZIndexProperty); }
            set { SetValue(MovePagePanelZIndexProperty, value); }
        }
        
        public Control DummyDragControl {
            get { return (Control)GetValue(DummyDragControlProperty); }
            set { SetValue(DummyDragControlProperty, value); }
        }
        
        public int DragedButtonNo {
            get { return (int)GetValue(DragedButtonNoProperty); }
            set { SetValue(DragedButtonNoProperty, value); }
        }

        public static readonly DependencyProperty AllowedEffectsProperty = DependencyProperty.Register("AllowedEffects", typeof(DragDropEffects), typeof(DragStartBehavior), new UIPropertyMetadata(DragDropEffects.All));
        public static readonly DependencyProperty DragDropDataProperty = DependencyProperty.Register("DragDropData", typeof(object), typeof(DragStartBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty MovePagePanelZIndexProperty = DependencyProperty.Register(nameof(MovePagePanelZIndex), typeof(int), typeof(DragStartBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty DummyDragControlProperty = DependencyProperty.Register(nameof(DummyDragControl), typeof(Control), typeof(DragStartBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty DragedButtonNoProperty = DependencyProperty.Register(nameof(DragedButtonNo), typeof(int), typeof(DragStartBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty CurrentPageNoProperty = DependencyProperty.Register(nameof(CurrentPageNo), typeof(int), typeof(DragStartBehavior), new PropertyMetadata(null));

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
                MovePagePanelZIndex = 1;
                try {
                    if (_dragGhost == null) {
                        var c = sender as Control;
                        DragedButtonNo = int.Parse(c.Tag.ToString());

                        _dragGhost = new DragGhost(sender as UIElement, 0.5, e.GetPosition(sender as UIElement), DummyDragControl);
                        _dragGhost.Show();
                    }
                    DragDrop.DoDragDrop(AssociatedObject, $"{CurrentPageNo},{DragDropData}", AllowedEffects);
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
