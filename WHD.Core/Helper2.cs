using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WHD.Core
{
    public class Helper2
    {
        private static Random rnd = new Random();
        [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")] public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")] public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")] public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        [DllImport("user32.dll")] public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        //ShowWindow参数
        public const int SW_SHOWNORMAL = 1;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWNOACTIVATE = 4;
        //SendMessage参数
        public const int WM_KEYDOWN = 0X100;
        public const int WM_KEYUP = 0X101;
        public const int WM_SYSCHAR = 0X106;
        public const int WM_SYSKEYUP = 0X105;
        public const int WM_SYSKEYDOWN = 0X104;
        public const int WM_CHAR = 0X102;
        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// 释放鼠标左键
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;
        /// <summary>
        /// 按下鼠标右键
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x204;
        /// <summary>
        /// 释放鼠标右键
        /// </summary>
        public const int WM_RBUTTONUP = 0x205;

        public static void NatualSendKey(IntPtr hWnd, Keys key)
        {
            SendMessage(hWnd, WM_KEYDOWN, Convert.ToInt32(key), 0);
            Thread.Sleep(rnd.Next(20, 100));
            SendMessage(hWnd, WM_KEYUP, Convert.ToInt32(key), 0);
        }
        public static void NatualSendMouse(IntPtr hWnd, bool isLeft,int x,int y)
        {
            if (isLeft)
            {
                SendMessage(hWnd, WM_LBUTTONDOWN, 0, x << 4 + y);
                Thread.Sleep(rnd.Next(20, 60));
                SendMessage(hWnd, WM_LBUTTONUP, 0, x << 4 + y);
            }
            else
            {
                SendMessage(hWnd, WM_RBUTTONDOWN, 0, x << 4 + y);
                Thread.Sleep(rnd.Next(20, 60));
                SendMessage(hWnd, WM_RBUTTONUP, 0, x << 4 + y);
            }
        }
        public static void NatualSendMouseWheel(IntPtr hWnd)
        {
            Thread.Sleep(rnd.Next(20, 50));
            SendMessage(hWnd, (int)MsgType.WM_MOUSEWHEEL, 0X780000, 0X2C0025B);
            Thread.Sleep(rnd.Next(20, 50));
        }

        public static void SendSysKey(IntPtr hWnd, Keys key)
        {
            SendMessage(hWnd, WM_SYSKEYDOWN, Convert.ToInt32(key), 0);
            SendMessage(hWnd, WM_SYSKEYUP, Convert.ToInt32(key), 0);
        }

        public static void SendChar(IntPtr hWnd, char c)
        {
            SendMessage(hWnd, WM_CHAR, (int)c, 0);
        }
        //有问题
        public static Bitmap GetWindowCapture(IntPtr hWnd)
        {
            IntPtr hscrdc = GDI32.GetWindowDC(hWnd);
            GDI32.WinRECT windowRect = new GDI32.WinRECT();
            GDI32.GetWindowRect(hWnd, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hbitmap = GDI32.CreateCompatibleBitmap(hscrdc, width, height);
            IntPtr hmemdc = GDI32.CreateCompatibleDC(hscrdc);
            GDI32.SelectObject(hmemdc, hbitmap);
            GDI32.PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            GDI32.DeleteDC(hscrdc);//删除用过的对象
            GDI32.DeleteDC(hmemdc);//删除用过的对象
            return bmp;
        }
        public static Bitmap ScreenShotAllScreen()
        {
            IntPtr dc1 = GDI32.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            Console.WriteLine(dc1.ToString());
            //创建显示器的DC   
            Graphics g1 = Graphics.FromHdc(dc1);
            //由一个指定设备的句柄创建一个新的Graphics对象   
            Bitmap MyImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, g1);
            //根据屏幕大小创建一个与之相同大小的Bitmap对象   
            Graphics g2 = Graphics.FromImage(MyImage);
            //获得屏幕的句柄   
            IntPtr dc3 = g1.GetHdc();
            //获得位图的句柄   
            IntPtr dc2 = g2.GetHdc();
            //把当前屏幕捕获到位图对象中   
            GDI32.BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, CopyPixelOperation.SourceCopy);

            GDI32.DeleteDC(dc1);
            //把当前屏幕拷贝到图中   
            g1.ReleaseHdc(dc3);
            //释放屏幕句柄   
            g2.ReleaseHdc(dc2);
            return MyImage;
        }
        public static Bitmap ScreenShot(IntPtr hWnd)
        {

            IntPtr sourceDC = GDI32.GetWindowDC(hWnd);
            //创建显示器的DC   
            Graphics gSource = Graphics.FromHdc(sourceDC);
            //由一个指定设备的句柄创建一个新的Graphics对象   


            GDI32.WinRECT windowRect = new GDI32.WinRECT();
            GDI32.GetWindowRect(hWnd, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;


            Bitmap MyImage = new Bitmap(width, height, gSource);
            //根据屏幕大小创建一个与之相同大小的Bitmap对象   
            Graphics gDest = Graphics.FromImage(MyImage);
            //获得屏幕的句柄   
            IntPtr hdcSource = gSource.GetHdc();
            //获得位图的句柄   
            IntPtr hdcDest = gDest.GetHdc();
            //把当前屏幕捕获到位图对象中   
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSource, 0, 0, CopyPixelOperation.SourceCopy);

            GDI32.ReleaseDC(hWnd, sourceDC);
            //把当前屏幕拷贝到图中   
            gSource.ReleaseHdc(hdcSource);
            //释放屏幕句柄   
            gDest.ReleaseHdc(hdcDest);
            return MyImage;
        }
        //public static Bitmap ScreenShot2(IntPtr hWnd)
        //{
            //IntPtr sourceDC = GDI32.GetWindowDC(hWnd);
            ////创建显示器的DC   
            //Graphics gSource = Graphics.FromHdc(sourceDC);
            ////由一个指定设备的句柄创建一个新的Graphics对象

            //GDI32.WinRECT windowRect = new GDI32.WinRECT();
            //GDI32.GetWindowRect(hWnd, ref windowRect);
            //int width = windowRect.right - windowRect.left;
            //int height = windowRect.bottom - windowRect.top;

            //Bitmap MyImage = new Bitmap(width, height);
            ////根据屏幕大小创建一个与之相同大小的Bitmap对象   
            //Graphics gDest = Graphics.FromImage(MyImage);
            ////获得屏幕的句柄   
            //IntPtr hdcSource = gSource.GetHdc();
            ////获得位图的句柄   
            //IntPtr hdcDest = gDest.GetHdc();
            ////把当前屏幕捕获到位图对象中   
            //GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSource, 0, 0, CopyPixelOperation.SourceCopy);
            ////把当前屏幕拷贝到图中   
            //gSource.ReleaseHdc(hdcSource);
            ////释放屏幕句柄   
            //gDest.ReleaseHdc(hdcDest);

            //var bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            //using (Graphics g = Graphics.FromImage(bitmap))
            //{
            //    g.
            //    g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), CopyPixelOperation.SourceCopy);
            //}
            //return bitmap;

        //}
        public static Bitmap ScreenShot2()
        {
            var bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), CopyPixelOperation.SourceCopy);
            }
            return bitmap;

        }

    }
}
