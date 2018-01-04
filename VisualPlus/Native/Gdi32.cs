namespace VisualPlus.Native
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Security;

    #endregion

    [SuppressUnmanagedCodeSecurity]
    internal static class Gdi32
    {
        #region Events

        [Description("The BitBlt function performs a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.")]
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [Description("The CreateRoundRectRgn function creates a rectangular region with rounded corners.")]
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        #endregion
    }
}