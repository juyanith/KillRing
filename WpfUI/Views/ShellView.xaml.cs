using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfUI.Models;
using WpfUI.ViewModels;

namespace WpfUI.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();

            // Start minimized.
            Hide();
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
        }

        public void ClipboardMonitorStart()
        {
            NativeMethods.AddClipboardFormatListener(windowHandle);
        }

        public void ClipboardMonitorStop()
        {
            NativeMethods.RemoveClipboardFormatListener(windowHandle);
        }

        public void RegisterHotKey()
        {
            NativeMethods.RegisterHotKey(windowHandle, NativeMethods.HOTKEY_ID, NativeMethods.MOD_CTRL | NativeMethods.MOD_SHIFT, NativeMethods.VK_V);
        }

        public void UnregisterHotKey()
        {
            NativeMethods.UnregisterHotKey(windowHandle, NativeMethods.HOTKEY_ID);
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_CLIPBOARDUPDATE:
                    (DataContext as ShellViewModel)?.ClipboardUpdated();
                    handled = false;
                    break;

                case NativeMethods.WM_HOTKEY:
                    switch (wparam.ToInt32())
                    {
                        case NativeMethods.HOTKEY_ID:
                            (DataContext as ShellViewModel)?.SetClipboard();
                            handled = true;
                            break;
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            windowHandle = new WindowInteropHelper(this).EnsureHandle();
            HwndSource.FromHwnd(windowHandle)?.AddHook(HwndHandler);
            ClipboardMonitorStart();
            RegisterHotKey();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UnregisterHotKey();
            ClipboardMonitorStop();
            HwndSource.FromHwnd(windowHandle)?.RemoveHook(HwndHandler);
        }

        private IntPtr windowHandle;
    }
}
