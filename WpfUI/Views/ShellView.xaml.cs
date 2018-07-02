using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        }

        public void ClipboardMonitorStart()
        {
            NativeMethods.AddClipboardFormatListener(windowHandle);
        }

        public void ClipboardMonitorStop()
        {
            NativeMethods.RemoveClipboardFormatListener(windowHandle);
        }

        public void RegisterHotKeys()
        {
            NativeMethods.RegisterHotKey(windowHandle, CLEAR_TEXT_HOTKEY_ID, NativeMethods.MOD_ALT | NativeMethods.MOD_CTRL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_NOREPEAT, NativeMethods.VK_DELETE);
            NativeMethods.RegisterHotKey(windowHandle, SET_CLIP_HOTKEY_ID, NativeMethods.MOD_CTRL | NativeMethods.MOD_SHIFT | NativeMethods.MOD_NOREPEAT, NativeMethods.VK_V);
        }

        public void UnregisterHotKeys()
        {
            NativeMethods.UnregisterHotKey(windowHandle, CLEAR_TEXT_HOTKEY_ID);
            NativeMethods.UnregisterHotKey(windowHandle, SET_CLIP_HOTKEY_ID);
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_CLIPBOARDUPDATE:
                    Debug.WriteLine("    WM_CLIPBOARDUPDATE.");
                    ViewModel?.ClipboardUpdated();
                    handled = false;
                    break;

                case NativeMethods.WM_HOTKEY:
                    switch (wparam.ToInt32())
                    {
                        case CLEAR_TEXT_HOTKEY_ID:
                            Debug.WriteLine("    CLEAR_TEXT_HOTKEY_ID.");
                            ViewModel?.ClearEntries();
                            handled = true;
                            break;

                        case SET_CLIP_HOTKEY_ID:
                            Debug.WriteLine("    SET_CLIP_HOTKEY_ID.");
                            ViewModel?.SetClipboard();
                            handled = true;
                            break;
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        private void ShellWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowInTaskbar = Visibility == Visibility.Visible;
        }

        private void ShellWindow_StateChanged(object sender, EventArgs e)
        {
            Visibility = WindowState == WindowState.Minimized ? Visibility.Hidden : Visibility.Visible;
        }

        private void ShellWindow_SourceInitialized(object sender, EventArgs e)
        {
            windowHandle = new WindowInteropHelper(this).EnsureHandle();
            HwndSource.FromHwnd(windowHandle)?.AddHook(HwndHandler);
            ClipboardMonitorStart();
            RegisterHotKeys();
        }

        private void ShellWindow_Closed(object sender, EventArgs e)
        {
            UnregisterHotKeys();
            ClipboardMonitorStop();
            HwndSource.FromHwnd(windowHandle)?.RemoveHook(HwndHandler);
        }

        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        private IntPtr windowHandle;
        private const int SET_CLIP_HOTKEY_ID = 9000;
        private const int CLEAR_TEXT_HOTKEY_ID = 9001;
    }
}
