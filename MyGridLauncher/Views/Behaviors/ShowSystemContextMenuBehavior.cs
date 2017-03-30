using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace MyToolsLauncher.Views.Behaviors {
    public class ShowSystemContextMenuBehavior : Behavior<Window> {
        public ContextMenu SystemContextMenu {
            get { return (ContextMenu)GetValue(SystemContextMenuProperty); }
            set { SetValue(SystemContextMenuProperty, value); }
        }

        public static readonly DependencyProperty SystemContextMenuProperty = DependencyProperty.Register(nameof(SystemContextMenu), typeof(ContextMenu), typeof(ShowSystemContextMenuBehavior), new PropertyMetadata(null));


        public ItemsControl SystemContextMenuItems {
            get { return (ItemsControl)GetValue(SystemContextMenuItemsProperty); }
            set { SetValue(SystemContextMenuItemsProperty, value); }
        }

        public static readonly DependencyProperty SystemContextMenuItemsProperty = DependencyProperty.Register(nameof(SystemContextMenuItems), typeof(ItemsControl), typeof(ShowSystemContextMenuBehavior), new PropertyMetadata(null));

        protected override void OnAttached() {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching() {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e) {
            IntPtr windowhandle = new WindowInteropHelper(AssociatedObject).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        private const uint WM_SYSTEMMENU = 0xa4;
        private const uint WP_SYSTEMMENU = 0x02;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            if ((msg == WM_SYSTEMMENU) && (wParam.ToInt32() == WP_SYSTEMMENU)) {
                ShowContextMenu();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void ShowContextMenu() {
            if (SystemContextMenu == null) {
                return;
            }
            SystemContextMenu.IsOpen = true;
        }
    }
}
