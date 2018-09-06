using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WHD.Core;

namespace WHD.Wowpet
{
    class Program
    {
        static void Main(string[] args)
        {
            //SetWindowPos(PW, (IntPtr)(-1), 0, 0, 0, 0, 0x0040 | 0x0001); //设置窗口位置
            //SetCursorPos(476, 177); //设置鼠标位置
            //mouse_event(0x0002, 0, 0, 0, 0); //模拟鼠标按下操作
            //mouse_event(0x0004, 0, 0, 0, 0); //
            //SendKeys.Send("{TAB}"); //模拟键盘输入TAB
            //SendKeys.Send(_GamePass); //模拟键盘输入游戏密码
            //SendKeys.Send("{ENTER}"); //模拟键盘输入ENTER
            var process = new WowProcess();
            if (process.IsVailed)
            {
                process.StartWork();
            }
            else
            {
                Console.Write("Cannot Load Process");
            }
        }
    }
}
