using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.Models
{
    internal static class NativeMethods
    {
        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CTRL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_WIN = 0x0008;
        public const int MOD_NOREPEAT = 0x4000;

        public const uint VK_V = 0x0056;
        public const uint VK_PRINT = 0x002a;
        public const uint VK_INSERT = 0x002d;
        public const uint VK_DELETE = 0x002e;
        public const uint VK_LCTRL = 0x00A2;
        public const uint VK_LALT = 0x00A4;
        public const uint VK_LWIN = 0x005B;

        const uint KEYEVENTF_EXTENDEDKEY = 0x0001; // key down
        const uint KEYEVENTF_KEYUP = 0x0002;

        // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
        public const int WM_CLIPBOARDUPDATE = 0x031D;

        public const int WM_HOTKEY = 0x0312;

        public static IntPtr HWND_MESSAGE = new IntPtr(-3);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public static void SendCtrlV()
        {
            keybd_event((byte)VK_LCTRL, 0x1d, KEYEVENTF_EXTENDEDKEY | 0, 0);
            keybd_event((byte)VK_V, 0x2f, 0, KEYEVENTF_EXTENDEDKEY | 0);
            keybd_event((byte)VK_V, 0x2f, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            keybd_event((byte)VK_LCTRL, 0x1d, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}
