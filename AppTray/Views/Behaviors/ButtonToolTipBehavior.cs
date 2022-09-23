using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;

namespace AppTray.Views.Behaviors
{
    public class ButtonToolTipBehavior : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ToolTipOpening += AssociatedObject_ToolTipOpening;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ToolTipOpening -= AssociatedObject_ToolTipOpening;
            base.OnDetaching();
        }

        private void AssociatedObject_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (!IsTextTrimmed())
            {
                e.Handled = true;
            }
        }

        private bool IsTextTrimmed()
        {
            if (!AssociatedObject.IsVisible)
            {
                return false;
            }
            if (AssociatedObject.ActualWidth == 0)
            {
                return false;
            }

            var textEndScrPt = AssociatedObject.PointToScreen(new Point(AssociatedObject.ActualWidth - 1, AssociatedObject.ActualHeight - 1));
            var textEndRelPt = AssociatedObject.PointFromScreen(textEndScrPt);

            var hitTest = VisualTreeHelper.HitTest(AssociatedObject, textEndRelPt);
            return hitTest == null;
        }
    }
}
