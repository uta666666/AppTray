using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyToolsLauncher.Views.Behaviors {
    public class DragMoveBehavior : Behavior<System.Windows.Controls.Grid> {
        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_PreviewMouseLeftButtonUp;
        }

        System.Windows.Controls.Grid dragItem;
        System.Windows.Point dragStartPos;
        DragAdorner dragGhost;

        private bool _isMouseDown;

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            // マウスダウンされたアイテムを記憶
            dragItem = sender as System.Windows.Controls.Grid;
            // マウスダウン時の座標を取得
            dragStartPos = e.GetPosition(dragItem);

            _isMouseDown = true;
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e) {
            var lbi = sender as System.Windows.Controls.Grid;
            if (_isMouseDown) {
                if (dragGhost == null && dragItem == lbi) {
                    var nowPos = e.GetPosition(lbi);
                    if (Math.Abs(nowPos.X - dragStartPos.X) > SystemParameters.MinimumHorizontalDragDistance ||
                        Math.Abs(nowPos.Y - dragStartPos.Y) > SystemParameters.MinimumVerticalDragDistance) {
                        //listBox.AllowDrop = true;

                        var layer = AdornerLayer.GetAdornerLayer(lbi);
                        dragGhost = new DragAdorner(lbi, lbi, 0.5, dragStartPos);
                        layer.Add(dragGhost);
                        DragDrop.DoDragDrop(lbi, lbi, DragDropEffects.Move);
                        layer.Remove(dragGhost);
                        dragGhost = null;
                        //dargItem = null;

                        //listBox.AllowDrop = false;
                    }
                }
            }
        }

        private void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            _isMouseDown = false;
        }
    }

    public class DragAdorner : Adorner {
        protected UIElement _child;
        protected double XCenter;
        protected double YCenter;

        public DragAdorner(UIElement owner) : base(owner) { }

        public DragAdorner(UIElement owner, UIElement adornElement, double opacity, System.Windows.Point dragPos)
            : base(owner) {
            var _brush = new VisualBrush(adornElement) { Opacity = opacity };
            var b = VisualTreeHelper.GetDescendantBounds(adornElement);
            var r = new System.Windows.Shapes.Rectangle() { Width = (int)b.Width, Height = (int)b.Height };

            XCenter = dragPos.X;// r.Width / 2;
            YCenter = dragPos.Y;// r.Height / 2;

            //r.Fill = _brush;
            _child = r;
        }


        private double _leftOffset;
        public double LeftOffset {
            get { return _leftOffset; }
            set {
                _leftOffset = value - XCenter;
                UpdatePosition();
            }
        }

        private double _topOffset;
        public double TopOffset {
            get { return _topOffset; }
            set {
                _topOffset = value - YCenter;
                UpdatePosition();
            }
        }

        private void UpdatePosition() {
            var adorner = this.Parent as AdornerLayer;
            if (adorner != null) {
                adorner.Update(this.AdornedElement);
            }
        }

        protected override Visual GetVisualChild(int index) {
            return _child;
        }

        protected override int VisualChildrenCount {
            get { return 1; }
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size finalSize) {
            _child.Measure(finalSize);
            return _child.DesiredSize;
        }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) {

            _child.Arrange(new Rect(_child.DesiredSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform) {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
            return result;
        }
    }
}
