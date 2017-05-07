using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AppTray.Views.Behaviors {
    public class PageNavigateBehavior : Behavior<StackPanel> {
        public int PageCount {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        public int CurrentPageCount {
            get { return (int)GetValue(CurrentPageCountProperty); }
            set { SetValue(CurrentPageCountProperty, value); }
        }

        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(nameof(PageCount), typeof(int), typeof(PageNavigateBehavior), new PropertyMetadata(1, OnPageCountPropertyChanged));
        public static readonly DependencyProperty CurrentPageCountProperty = DependencyProperty.Register(nameof(CurrentPageCount), typeof(int), typeof(PageNavigateBehavior), new PropertyMetadata(1, OnCurrentPageCountPropertyChanged));

        private static void OnPageCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var behavior = sender as PageNavigateBehavior;
            if (behavior == null) {
                return;
            }
            if (behavior.AssociatedObject == null) {
                return;
            }
            behavior.AssociatedObject.Children.Clear();
            for (int i = 0; i < behavior.PageCount; i++) {
                var rectangle = MakeRectangle();
                behavior.AssociatedObject.Children.Add(rectangle);
            }
        }

        private static void OnCurrentPageCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var behavior = sender as PageNavigateBehavior;
            if (behavior == null) {
                return;
            }
            if (behavior.AssociatedObject == null) {
                return;
            }
            for (int i = 0; i < behavior.PageCount; i++) {
                var rectangle = behavior.AssociatedObject.Children[i] as Rectangle;
                if (i == behavior.CurrentPageCount - 1) {
                    rectangle.Fill = new SolidColorBrush(Colors.Blue);
                } else {
                    rectangle.Fill = new SolidColorBrush(Colors.Black);
                }
            }

            behavior.AssociatedObject.Visibility = Visibility.Visible;

            _startTime = DateTime.Now;
            if (_timer == null) {
                _timer = new DispatcherTimer() {
                    Interval = new TimeSpan(100 * TimeSpan.TicksPerMillisecond)
                };
                _timer.Tick += (sender2, e2) => {
                    var span = DateTime.Now - _startTime;
                    if (span >= new TimeSpan(1000 * TimeSpan.TicksPerMillisecond)) {
                        behavior.AssociatedObject.Visibility = Visibility.Collapsed;
                        _timer.Stop();
                    }
                };
            }
            if (!_timer.IsEnabled) {
                _timer.Start();
            }
        }

        private static DateTime _startTime;
        private static DispatcherTimer _timer;

        protected override void OnAttached() {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
            for (int i = 0; i < PageCount; i++) {
                var rectangle = MakeRectangle();
                AssociatedObject.Children.Add(rectangle);
            }
        }

        private static Rectangle MakeRectangle() {
            return new Rectangle() {
                Width = 20,
                Height = 10,
                RadiusX = 5,
                RadiusY = 5,
                Margin = new Thickness(10),
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = new SolidColorBrush(Colors.White)
            };
        }
    }
}
