using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WHD.Core;

namespace WHD.Wowpet
{
    public partial class WowProcess : AbstractProcess
    {
        /// <summary>
        /// 判断状态的参数
        /// </summary>
        private double avgHistFactor = 15;
        private double meanFactor = 0.6;
        private double stddevFactor = 0.05;
        private double lastavgHistResult = 0;
        private double lastmeanResult = 0;
        private double laststddevResult = 0;
        private int battleCount = 0;

        public PetStatus PetStatus { get; }

        Thread battleThread;
        Bitmap petBattleBmp;

        public ManualResetEvent mre = new ManualResetEvent(false);
        public WowProcess() : base("魔兽世界")
        {
            battleThread = new Thread(BattleProc);
            battleThread.Start();

            string petBattleBmpPath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\PetBattle-nb.bmp";
            petBattleBmp = new Bitmap(petBattleBmpPath);

            PetStatus = new PetStatus();
        }

        /// <summary>
        /// Battling Thread
        /// </summary>
        void BattleProc()
        {
            while (true)
            {
                //如果不被主动停止，会继续执行
                mre.WaitOne();
                NatualSendKey(Environment.KeyAutoBattle);
                //NatualSendMouseWheel();
                RandomSleep(400, 600);
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
            Console.WriteLine("1.NPC 2.PetNpc 3.FortressNpc");
            var c = Console.ReadLine();
            if (c.Equals("1"))
            {
                Console.WriteLine("Input time count");
                var time = Console.ReadLine();
                
                return new WorldQuestNpcBattlePolicy(this,int.Parse(time)).Run();
            }
            else if (c.Equals("2"))
            {
                Console.WriteLine("Input time count");
                var time = Console.ReadLine();

                return new PetNpcBattlePolicy(this, int.Parse(time)).Run();
            }
            else if (c.Equals("3"))
            {
                Console.WriteLine("Input time count");
                var time = Console.ReadLine();

                return new FortressNpcBattlePolicy(this, int.Parse(time)).Run();
            }
            else
            {
                return 1;
            }
        }
        public bool IsBattlingMode()
        {
            try
            {
                double avgHistResult, meanResult, stddevResult;
                //sourceBmp.Save(sourceBmpPath);
                using (Bitmap sourceBmp = Helper2.ScreenShotAllScreen())
                {
                    OpenCV.SimpleCheckImageLike(sourceBmp, petBattleBmp, out avgHistResult, out meanResult, out stddevResult);
                }
                if (Math.Abs(avgHistResult - lastavgHistResult) > 10)
                {
                    lastmeanResult = meanResult;
                    laststddevResult = stddevResult;
                    Console.WriteLine($"IsBattlingMode - 不同值:{avgHistResult} 平均值:{meanResult} 方差:{stddevResult}");
                }
                return avgHistResult < avgHistFactor && meanResult < meanFactor;
            }
            catch (Exception e)
            {
                ConsoleAndLogInfo(e.ToString());
                ConsoleAndLogInfo(e.Message);
                ConsoleAndLogInfo(e.StackTrace);
                return false;
            }
        }
    }
}
