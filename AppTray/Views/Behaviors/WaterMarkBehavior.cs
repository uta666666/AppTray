using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AppTray.Views.Behaviors
{
    public class WaterMarkBehavior : Behavior<AutoCompleteBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.DropDownClosed += AssociatedObject_DropDownClosed;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            AssociatedObject.Initialized -= AssociatedObject_Initialized;
            AssociatedObject.DropDownClosed -= AssociatedObject_DropDownClosed;

            base.OnDetaching();
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            _brash = AssociatedObject.Background;
        }

        private Brush _brash;

        private void AssociatedObject_TextChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text) && !AssociatedObject.IsDropDownOpen)
            {
                AssociatedObject.Background = _brash;
            }
            else
            {
                AssociatedObject.Background = new SolidColorBrush(Color.FromArgb(255,10,10,10));
            }
        }

        private void AssociatedObject_DropDownClosed(object sender, System.Windows.RoutedPropertyChangedEventArgs<bool> e)
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text))
            {
                AssociatedObject.Background = _brash;
            }
        }
    }
}
