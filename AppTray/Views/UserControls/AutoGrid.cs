using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppTray.Views.UserControls
{
    public class AutoGrid : Grid
    {
        public int ColumnDefinitionWidth
        {
            get { return (int)GetValue(ColumnDefinitionWidthProperty); }
            set { SetValue(ColumnDefinitionWidthProperty, value); }
        }

        public int GridColumnCount
        {
            get { return (int)GetValue(GridColumnCountProperty); }
            set { SetValue(GridColumnCountProperty, value); }
        }

        public int RowDefinitionHeight
        {
            get { return (int)GetValue(RowDefinitionHeightProperty); }
            set { SetValue(RowDefinitionHeightProperty, value); }
        }

        public int GridRowCount
        {
            get { return (int)GetValue(GridRowCountProperty); }
            set { SetValue(GridRowCountProperty, value); }
        }


        public static readonly DependencyProperty ColumnDefinitionWidthProperty =
                                    DependencyProperty.Register(nameof(ColumnDefinitionWidth),
                                        typeof(int),
                                        typeof(AutoGrid),
                                        new PropertyMetadata(50));

        public static readonly DependencyProperty GridColumnCountProperty =
                                    DependencyProperty.RegisterAttached(nameof(GridColumnCount),
                                        typeof(int),
                                        typeof(AutoGrid),
                                        new PropertyMetadata(0, new PropertyChangedCallback(OnGridColumnCountChanged)));

        public static readonly DependencyProperty RowDefinitionHeightProperty =
                                     DependencyProperty.Register(nameof(RowDefinitionHeight),
                                         typeof(int),
                                         typeof(AutoGrid),
                                         new PropertyMetadata(-1));

        public static readonly DependencyProperty GridRowCountProperty =
                                    DependencyProperty.RegisterAttached(nameof(GridRowCount),
                                        typeof(int),
                                        typeof(AutoGrid),
                                        new PropertyMetadata(1, new PropertyChangedCallback(OnGridRowCountChanged)));




        private static void OnGridRowCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid) || (int)e.NewValue < 0)
            {
                return;
            }

            Grid grid = (Grid)d;

            grid.RowDefinitions.Clear();

            int heightProperty = (int)d.GetValue(RowDefinitionHeightProperty);

            for (int i = 0; i < (int)e.NewValue; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                if (heightProperty < 0)
                {
                    rowDefinition.Height = new GridLength(14, GridUnitType.Pixel);
                }
                else if (heightProperty > 0)
                {
                    rowDefinition.Height = new GridLength(heightProperty, GridUnitType.Pixel);
                }
                else if (heightProperty == 0)
                {
                    rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                }
                grid.RowDefinitions.Add(rowDefinition);
            }
        }

        private static void OnGridColumnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid) || (int)e.NewValue < 0)
            {
                return;
            }

            Grid grid = (Grid)d;

            grid.ColumnDefinitions.Clear();

            int widthProperty = (int)d.GetValue(ColumnDefinitionWidthProperty);

            for (int i = 0; i < (int)e.NewValue; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                if (widthProperty < 0)
                {
                    columnDefinition.Width = new GridLength(50, GridUnitType.Pixel);
                }
                else if (widthProperty > 0)
                {
                    columnDefinition.Width = new GridLength(widthProperty, GridUnitType.Pixel);
                }
                else if (widthProperty == 0)
                {
                    columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                }
                grid.ColumnDefinitions.Add(columnDefinition);
            }
        }
    }
}
