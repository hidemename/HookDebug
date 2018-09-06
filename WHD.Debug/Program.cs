using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WHD.Core;

namespace WHD.Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenCVDebug.Debug();


            var process = new TestProcess("Steam");
            var hWND = process.GetHandle();
            Bitmap bmp = Helper2.ScreenShotAllScreen();
            bmp.Save(@"Source.bmp");
        }
        public class TestProcess : AbstractProcess
        {
            public TestProcess(string name) : base(name)
            { }
            public override int StartWork()
            {
                throw new NotImplementedException();
            }
        }
    }
}
