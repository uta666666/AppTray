using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyToolsLauncher.Views.Behaviors {
    public class ShowWindowBehavior : Behavior<Window> {
        public double Top {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        public double Left {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(ShowWindowBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(ShowWindowBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.StateChanged += AssociatedObject_StateChanged;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.StateChanged -= AssociatedObject_StateChanged;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
            AssociatedObject.WindowState = WindowState.Minimized;
        }

        private void AssociatedObject_StateChanged(object sender, EventArgs e) {
            if (AssociatedObject.WindowState == WindowState.Normal) {
                //Point screenPoint = CursorInfo.GetCursorPosition();
                //Left = screenPoint.X - (AssociatedObject.Width / 2);
                //double diff = screenPoint.Y - SystemParameters.WorkArea.Height;
                //Top = screenPoint.Y - AssociatedObject.Height - diff;

                System.Drawing.Point dp = System.Windows.Forms.Cursor.Position;
                System.Windows.Point wp = new System.Windows.Point(dp.X, dp.Y);
                // マウス座標から論理座標に変換
                PresentationSource src = PresentationSource.FromVisual(AssociatedObject);
                System.Windows.Media.CompositionTarget ct = src.CompositionTarget;
                System.Windows.Point p = ct.TransformFromDevice.Transform(wp);
                Left = p.X - (AssociatedObject.Width / 2);
                double diff = p.Y - SystemParameters.WorkArea.Height;
                Top = p.Y - AssociatedObject.Height - diff;
            }
        }
    }
}
