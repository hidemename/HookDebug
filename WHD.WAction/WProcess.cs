using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WHD.Core;

namespace WHD.WAction
{
    public partial class WProcess : AbstractProcess
    {
        public ManualResetEvent mre = new ManualResetEvent(false);
        public WProcess() : base(Encoding.UTF8.GetString(Convert.FromBase64String("6a2U5YW95LiW55WM")))
        {
        }

        /// <summary>
        /// Battling Thread
        /// </summary>
        int FourPlusFour()
        {
            while (true)
            {
                ConsoleAndLogInfo("NextTurn");
                NatualSendKey(Keys.Tab);
                CastSkill(Keys.D2);
                CastSkill(Keys.D1);
                CastSkill(Keys.R);
                CastSkill(Keys.E);
                NatualSendKey(Keys.Tab);
                CastSkill(Keys.D1);
                CastSkill(Keys.D8);
                CastSkill(Keys.E);
                NatualSendKey(Keys.Tab);
                CastSkill(Keys.D2);
                CastSkill(Keys.D1);
                CastSkill(Keys.R);
                CastSkill(Keys.E);
                CastSkill(Keys.D8);
                NatualSendKey(Keys.Tab);
                RandomSleep(1800, 2000);
                CastSkill(Keys.D8);
                CastSkill(Keys.D);
                CastSkill(Keys.E);
            }
        }


        public override int StartWork()
        {

            //Console.WriteLine("Use Bandage? yes is 1");
            //var a = Console.ReadLine();
            //var useBandage = false;
            //if (a.Equals("1"))
            //{
            //    useBandage = true;
            //}
            Console.WriteLine("1.FourPlusFour");
            var c = Console.ReadLine();
            if (c.Equals("1"))
            {
                Console.WriteLine("Input time count");
                var time = Console.ReadLine();
                
                return FourPlusFour();
            }
            else
            {
                return 1;
            }
        }
        private void CastSkill(Keys key)
        {
            //ConsoleAndLogInfo("key");
            NatualSendKey(key);
            RandomSleep(1800, 2000);
        }
    }
}
