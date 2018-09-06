using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WHD.Core
{
    public abstract class AbstractProcess
    {
        protected IntPtr window = IntPtr.Zero;
        private Random rnd;
        public AbstractProcess(string windowName,int seed = 0)
        {
            window = Helper.FindWindow(windowName);
            rnd = (seed == 0 ? new Random() : new Random(seed));
        }
        public bool IsVailed
        {
            get
            {
                return !window.Equals(IntPtr.Zero);
            }
        }
        public abstract int StartWork();


        public void NatualSendKey(Keys key)
        {
            Helper2.NatualSendKey(window, key);
        }
        public void NatualSendMouse(bool isLeft,int x,int y)
        {
            Helper2.NatualSendMouse(window, isLeft, x, y);
        }
        public void NatualSendMouseWheel()
        {
            Helper2.NatualSendMouseWheel(window);
        }
        public IntPtr GetHandle()
        {
            return window;
        }
        public void RandomSleep(int min,int max)
        {
            Thread.Sleep(rnd.Next(min, max));
        }
        public void ConsoleAndLogInfo(string s)
        {
            Console.WriteLine(s);
            LogHelper.Info(s);
        }
    }
}
