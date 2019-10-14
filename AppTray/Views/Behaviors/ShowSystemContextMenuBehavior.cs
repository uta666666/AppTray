using AppTray.Commons;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace AppTray.Views.Behaviors {
    public class ShowSystemContextMenuBehavior : Behavior<Window> {
        //public ContextMenu SystemContextMenu {
        //    get { return (ContextMenu)GetValue(SystemContextMenuProperty); }
        //    set { SetValue(SystemContextMenuProperty, value); }
        //}

        //public static readonly DependencyProperty SystemContextMenuProperty = DependencyProperty.Register(nameof(SystemContextMenu), typeof(ContextMenu), typeof(ShowSystemContextMenuBehavior), new PropertyMetadata(null));


        //public ItemsControl SystemContextMenuItems {
        //    get { return (ItemsControl)GetValue(SystemContextMenuItemsProperty); }
        //    set { SetValue(SystemContextMenuItemsProperty, value); }
        //}

        //public static readonly DependencyProperty SystemContextMenuItemsProperty = DependencyProperty.Register(nameof(SystemContextMenuItems), typeof(ItemsControl), typeof(ShowSystemContextMenuBehavior), new PropertyMetadata(null));

        public List<SystemMenuItem> MenuItems {
            get { return (List<SystemMenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(nameof(MenuItems), typeof(List<SystemMenuItem>), typeof(ShowSystemContextMenuBehavior), new PropertyMetadata(null));

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

            IntPtr hSysMenu = GetSystemMenu(windowhandle, false);

            MENUITEMINFO item1 = new MENUITEMINFO();
            item1.cbSize = (uint)Marshal.SizeOf(item1);
            item1.fMask = MIIM_FTYPE;
            item1.fType = MFT_SEPARATOR;
            InsertMenuItem(hSysMenu, 5, true, ref item1);

            foreach (var menuItem in MenuItems) {
                if (menuItem.IsSeparator) {
                    MENUITEMINFO item3 = new MENUITEMINFO();
                    item3.cbSize = (uint)Marshal.SizeOf(item3);
                    item3.fMask = MIIM_FTYPE;
                    item3.fType = MFT_SEPARATOR;
                    InsertMenuItem(hSysMenu, 5, true, ref item3);
                    continue;
                }
                MENUITEMINFO item2 = new MENUITEMINFO();
                item2.cbSize = (uint)Marshal.SizeOf(item2);
                item2.fMask = MIIM_STRING | MIIM_ID;
                item2.wID = menuItem.MenuID;
                item2.dwTypeData = menuItem.MenuName;
                InsertMenuItem(hSysMenu, 6, true, ref item2);
            }

            //MENUITEMINFO item2 = new MENUITEMINFO();
            //item2.cbSize = (uint)Marshal.SizeOf(item2);
            //item2.fMask = MIIM_STRING | MIIM_ID;
            //item2.wID = MENU_ID_01;
            //item2.dwTypeData = "てすと1";
            //InsertMenuItem(hSysMenu, 6, true, ref item2);

            //MENUITEMINFO item3 = new MENUITEMINFO();
            //item3.cbSize = (uint)Marshal.SizeOf(item2);
            //item3.fMask = MIIM_STRING | MIIM_ID;
            //item3.wID = MENU_ID_02;
            //item3.dwTypeData = "てすと2";
            //InsertMenuItem(hSysMenu, 6, true, ref item3);


            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        //private const uint WM_SYSTEMMENU = 0xa4;
        //private const uint WP_SYSTEMMENU = 0x02;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            //if ((msg == WM_SYSTEMMENU) && (wParam.ToInt32() == WP_SYSTEMMENU)) {
            //    ShowContextMenu();
            //    handled = true;
            //}
            //return IntPtr.Zero;

            if (msg == WM_SYSCOMMAND) {
                uint menuid = (uint)(wParam.ToInt32() & 0xffff);

                if (MenuItems.Any(n => n.MenuID == menuid)) {
                    MenuItems.Where(n => n.MenuID == menuid).First().Command?.Execute(null);
                }
                //switch (menuid) {
                //    case MENU_ID_01:
                //        MenuItems.Where(n => n.MenuID == menuid).First().Command?.Execute(null);
                //        break;
                //    case MENU_ID_02:
                //        MessageBox.Show("てすと2が選択されました。");
                //        break;
                //}
            }
            return IntPtr.Zero;
        }

        //private void ShowContextMenu() {
        //    if (SystemContextMenu == null) {
        //        return;
        //    }
        //    SystemContextMenu.IsOpen = true;
        //}

        [StructLayout(LayoutKind.Sequential)]
        struct MENUITEMINFO {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;

            // return the size of the structure
            public static uint sizeOf {
                get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        private const uint MENU_ID_01 = 0x0001;
        private const uint MENU_ID_02 = 0x0002;

        private const uint MFT_BITMAP = 0x00000004;
        private const uint MFT_MENUBARBREAK = 0x00000020;
        private const uint MFT_MENUBREAK = 0x00000040;
        private const uint MFT_OWNERDRAW = 0x00000100;
        private const uint MFT_RADIOCHECK = 0x00000200;
        private const uint MFT_RIGHTJUSTIFY = 0x00004000;
        private const uint MFT_RIGHTORDER = 0x000002000;

        private const uint MFT_SEPARATOR = 0x00000800;
        private const uint MFT_STRING = 0x00000000;

        private const uint MIIM_FTYPE = 0x00000100;
        private const uint MIIM_STRING = 0x00000040;
        private const uint MIIM_ID = 0x00000002;

        private const uint WM_SYSCOMMAND = 0x0112;
    }
}
