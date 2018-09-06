using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WHD.Core
{
    class GDI32
    {
        #region GDI
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
                        string lpszDriver,//驱动名称
                        string lpszDevice,//设备名称
                        string lpszOutput,//无用，设为null
                        IntPtr lpInitData//任意的打印机数据
                        );

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(
            IntPtr hdc           // handle to DC
            );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDest, // 目标 DC的句柄
                                          int nXDest,
                                          int nYDest,
                                          int nWidth,
                                          int nHeight,
                                          IntPtr hdcSrc,  // 源DC的句柄
                                          int nXSrc,
                                          int nYSrc,
                                          CopyPixelOperation dwRop  // 光栅的处理数值
                                          );


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref WinRECT rect);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
                                        IntPtr hdc,         // handle to DC
                                        int nWidth,      // width of bitmap, in pixels
                                        int nHeight      // height of bitmap, in pixels
                                         );
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(
                                         IntPtr hdc // handle to DC
                                         );
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(
                                         IntPtr hdc,           // handle to DC
                                         IntPtr hgdiobj    // handle to object
                                         );
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
                                         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
                                         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
                                         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
                                         );
        public struct WinRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion
    }
}
