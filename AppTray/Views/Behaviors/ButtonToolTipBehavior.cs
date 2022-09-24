using AppTray.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;

namespace AppTray.Views.Behaviors
{
    public class ButtonToolTipBehavior : Behavior<TextBlock>
    {
        public string TextOriginal
        {
            get { return (string)GetValue(TextOriginalProperty); }
            set { SetValue(TextOriginalProperty, value); }
        }

        public static readonly DependencyProperty TextOriginalProperty = DependencyProperty.Register(nameof(TextOriginal), typeof(string), typeof(ButtonToolTipBehavior), new PropertyMetadata(null));

        public int RowCount
        {
            get { return (int)GetValue(RowCountPrperty); }
            set { SetValue(RowCountPrperty, value); }
        }

        public static readonly DependencyProperty RowCountPrperty = DependencyProperty.Register(nameof(RowCount), typeof(int), typeof(ButtonToolTipBehavior), new PropertyMetadata(null));



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
            double currentWidth = GetDrawingWidth(AssociatedObject.Text, AssociatedObject) / RowCount;
            double displayWidth = AssociatedObject.ActualWidth;

            if (currentWidth < displayWidth)
            {
                return false;
            }
            else
            {
                return true;
            }

            //if (!AssociatedObject.IsVisible)
            //{
            //    return false;
            //}
            //if (AssociatedObject.ActualWidth == 0)
            //{
            //    return false;
            //}

            //var textEndScrPt = AssociatedObject.PointToScreen(new Point(AssociatedObject.ActualWidth - 1, AssociatedObject.ActualHeight - 1));
            //var textEndRelPt = AssociatedObject.PointFromScreen(textEndScrPt);

            //var hitTest = VisualTreeHelper.HitTest(AssociatedObject, textEndRelPt);
            //return hitTest == null;
        }

        private double GetDrawingWidth(string str, TextBlock textBlock)
        {
            var formattedText = new FormattedText(
                str,
                CultureInfo.CurrentCulture,
                textBlock.FlowDirection,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                textBlock.FontSize,
                textBlock.Foreground,
                VisualTreeHelper.GetDpi(AssociatedObject).PixelsPerDip);

            return formattedText.Width;
        }
    }
}
