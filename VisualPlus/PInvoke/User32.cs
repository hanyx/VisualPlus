namespace VisualPlus.PInvoke
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security;

    using VisualPlus.Managers;

    #endregion

    [SuppressUnmanagedCodeSecurity]
    internal static class User32
    {
        #region Events

        [Description("Retrieves the cursor position in screen coordinates.")]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [Description("Retrieves the monitor information.")]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In] [Out] MonitorManager info);

        [Description("Retrieves the system menu.")]
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [Description("Retrieves the monitor from a window handle.")]
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [Description("You can use the ReleaseCapture() function to provide drag functionality.")]
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [Description("Sends a message to the window handle.")]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [Description("Displays a popup menu at a specified point. The function also tracks the menu, updating the selection highlight until the user either selects an item or otherwise closes the menu.")]
        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [Description("The WindowFromPoint function retrieves a handle to the window that contains the specified point.")]
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        #endregion
    }
}