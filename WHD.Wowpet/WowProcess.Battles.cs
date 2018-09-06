using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WHD.Core;

namespace WHD.Wowpet
{
    public partial class WowProcess
    {
        //public int SimpleBattle()
        //{
        //    int turn = 0;
        //    while (true)
        //    {
        //        Console.WriteLine($"开始下一场战斗 #{turn++}");
                
        //        Thread.Sleep(rnd.Next(200, 500));
        //        NatualSendKey(Keys.OemQuotes);
        //        Thread.Sleep(rnd.Next(200, 500));
        //        NatualSendKey(Keys.OemCloseBrackets);
        //        Thread.Sleep(rnd.Next(200, 500));
        //        NatualSendKey(_keyAutoMoveAndTalk);
        //        Thread.Sleep(rnd.Next(7000, 8000));
        //        for (int i = 0; i < 30; i++)
        //        {
        //            NatualSendKey(_keyAutoBattle);
        //            Thread.Sleep(rnd.Next(500, 1000));
        //        }
        //    }
        //}
        public void BattleAtSnow()
        {
            /*
            DateTime startTime = DateTime.Now;
            var endDay = startTime.AddDays(1);
            var endDayTime = new DateTime(endDay.Year, endDay.Month, endDay.Day, 6, 55, 00);

            SkillHealPet();
            DateTime healTime = DateTime.Now;
            while (true)
            {
                Console.WriteLine("开始下一场战斗 @{0}", DateTime.Now);
                Thread.Sleep(rnd.Next(800, 1000));


                TimeSpan totalBattleTimeSpan = DateTime.Now - healTime;
                if (totalBattleTimeSpan.TotalSeconds > 490)
                {

                    SkillHealPet();
                    healTime = DateTime.Now;
                }
                if (DateTime.Now > endDayTime)
                {
                    Console.Read();
                    return 0;
                }

                BattleWithPet();

                //start
                mre.Set();
                Console.WriteLine($"({battleCount})进入战斗界面");
                DateTime startBattleTime = DateTime.Now;
                while (true)
                {
                    if (!IsBattlingMode())
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }

                //wait
                mre.Reset();
                double totalSeconds = (DateTime.Now - startBattleTime).TotalSeconds;
                string RoundStatus = $"({battleCount})离开战斗界面，一共花费了" + totalSeconds + "s";
                Console.WriteLine(RoundStatus);
                Thread.Sleep(rnd.Next(1000, 1500));
            }*/
        }
        public int BattleOutsideNpc(int turn,bool useBandage)
        {
            ConsoleAndLogInfo($"BattleOutsideNpc:{turn}");
            var now = DateTime.Now;
            var endDayTime = new DateTime(now.Year, now.Month, now.Day, 6, 55, 00);
            if (DateTime.Now.Hour >= 7)
            {
                endDayTime = endDayTime.AddDays(1);
            }

            SkillHealPet();
            while (true)
            {
                for (int i = 0; i < turn; i++)
                {
                    if (DateTime.Now > endDayTime)
                    {
                        ConsoleAndLogInfo($"到了刷新时间，退出");
                        Console.Read();
                        return 0;
                    }
                    battleCount++;
                    ConsoleAndLogInfo($"({battleCount})开始寻找NPC");

                    var LookingForCount = 0;
                    while (true)
                    {
                        LookingForCount++;
                        LogHelper.Info("BattleWithNpc");
                        BattleWithNpc();
                        if (IsBattlingMode())
                        {
                            break;
                        }
                        else
                        {
                            if (petStatus.CheckIfCanHeal())
                            {
                                SkillHealPet();
                                i = 0;
                            }
                        }

                        if (LookingForCount>100)
                        {
                            ConsoleAndLogInfo($"无法战斗100次，退出");
                            Console.Read();
                            return 0;
                        }
                    }
                    ConsoleAndLogInfo($"({battleCount})寻找NPC完成，进入战斗");
                    //start
                    mre.Set();
                    string status = $"({battleCount})进入战斗界面";
                    ConsoleAndLogInfo(status);
                    DateTime startBattleTime = DateTime.Now;
                    while (true)
                    {
                        if (!IsBattlingMode())
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    //wait
                    mre.Reset();
                    double TotalSeconds = (DateTime.Now - startBattleTime).TotalSeconds;
                    string RoundStatus = $"({battleCount})战斗结束，Round#{i + 1} 一共花费了" + TotalSeconds + "s";
                    ConsoleAndLogInfo(RoundStatus);
                    Thread.Sleep(2000);

                }
                ConsoleAndLogInfo("等待恢复CD中");
                if (petStatus.CheckIfCanHeal())
                {
                    SkillHealPet();
                }
                else
                {
                    if (useBandage)
                    {
                        BandageHealPet();
                        ConsoleAndLogInfo("使用绷带");
                        continue;
                    }
                    else
                    {
                        double sleepTime = 500 - petStatus.GetLastHealSeconds() + 5;
                        Thread.Sleep((int)sleepTime * 1000);
                        ConsoleAndLogInfo("等待恢复CD完成");
                        SkillHealPet();
                    }
                }
            }
        }
        public void BattleAtHomePet(int turn)
        {
            /*
            DateTime startTime = DateTime.Now;
            ConsoleAndLogInfo($"BattleAtHomePet:{turn}");
            var endDay = startTime.AddDays(1);
            var endDayTime = new DateTime(endDay.Year, endDay.Month, endDay.Day, 4, 30, 00);

            HealByNpc();

            DateTime healTime = DateTime.Now;
            while (true)
            {
                for (int i = 0; i < turn; i++)
                {
                    battleCount++;
                    ConsoleAndLogInfo($"({battleCount})开始寻找NPC");
                    while (true)
                    {
                        LogHelper.Info("BattleWithHomePet");
                        if (i == 0)
                        {
                            BattleWithHomePet(668, 398);
                        }
                        else
                        {
                            BattleWithHomePet();
                        }
                        if (IsBattlingMode())
                        {
                            break;
                        }
                    }
                    ConsoleAndLogInfo($"({battleCount})寻找NPC完成，进入战斗");
                    //start
                    mre.Set();
                    string status = $"({battleCount})进入战斗界面";
                    ConsoleAndLogInfo(status);
                    DateTime startBattleTime = DateTime.Now;
                    while (true)
                    {
                        if (!IsBattlingMode())
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    //wait
                    mre.Reset();
                    string RoundStatus = $"({battleCount})战斗结束，Round#{i + 1} 一共花费了" + (DateTime.Now - startBattleTime).TotalSeconds + "s";
                    ConsoleAndLogInfo(RoundStatus);
                    Thread.Sleep(2000);

                }
                ConsoleAndLogInfo("寻找NPC治疗");
                HealByNpc();
                ConsoleAndLogInfo("寻找NPC治疗完成");
                healTime = DateTime.Now;
            }*/
        }

        public void BattleAtSHL()
        {
            /*
            DateTime startTime = DateTime.Now;
            ConsoleAndLogInfo($"BattleAtSHL");
            var endDay = startTime.AddDays(1);
            var endDayTime = new DateTime(endDay.Year, endDay.Month, endDay.Day, 4, 30, 00);

            SkillHealPet();

            DateTime healTime = DateTime.Now;
            while (true)
            {
                battleCount++;
                ConsoleAndLogInfo($"({battleCount})开始寻找NPC");
                while (true)
                {
                    LogHelper.Info("BattleWithPet");
                    BattleWithPet();
                    if (IsBattlingMode())
                    {
                        break;
                    }
                }
                ConsoleAndLogInfo($"({battleCount})寻找NPC完成，进入战斗");
                //start
                mre.Set();
                string status = $"({battleCount})进入战斗界面";
                ConsoleAndLogInfo(status);
                DateTime startBattleTime = DateTime.Now;
                while (true)
                {
                    if (!IsBattlingMode())
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
                //wait
                mre.Reset();
                string RoundStatus = $"({battleCount})战斗结束， 一共花费了" + (DateTime.Now - startBattleTime).TotalSeconds + "s";
                ConsoleAndLogInfo(RoundStatus);
                Thread.Sleep(2000);


                TimeSpan totalBattleTimeSpan = DateTime.Now - healTime;
                if (totalBattleTimeSpan.TotalSeconds > 482)
                {
                    ConsoleAndLogInfo("8MIN后治疗宠物");
                    SkillHealPet();
                    healTime = DateTime.Now;
                }
            }*/
        }
        /// <summary>
        /// 四风谷的战斗
        /// </summary>
        /// <returns></returns>
        public void BattleAtValleyoftheFourWinds()
        {
            /*
            bool wait = true;
            while (true)
            {
                Console.WriteLine("开始下一场战斗 @{0}", DateTime.Now);
                Thread.Sleep(rnd.Next(800, 1000));

                SkillHealPet();
                //heal
                DateTime healTime = DateTime.Now;
                //if (healTime > new DateTime(2018, 8, 8, 4, 50, 0, DateTimeKind.Local))
                //{
                //    Console.Read();
                //    Console.WriteLine("退出");
                //    return 0;
                //}
                for (int i = 0; i < 7; i++)
                {
                    BattleWithPet();

                    //start
                    mre.Set();
                    Console.WriteLine($"({battleCount})进入战斗界面");
                    DateTime startBattleTime = DateTime.Now;
                    while (true)
                    {
                        if (!IsBattlingMode())
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }

                    //wait
                    mre.Reset();
                    string RoundStatus = $"({battleCount})离开战斗界面，Round#{i + 1} 一共花费了" + (DateTime.Now - startBattleTime).TotalSeconds + "s";
                    Console.WriteLine(RoundStatus);
                    if (wait)
                    {
                        //MessageBox.Show(RoundStatus, "", MessageBoxButtons.OK);
                    }
                    Thread.Sleep(rnd.Next(1000, 1500));
                }
                if (wait)
                {
                    Console.WriteLine("等待恢复中");
                    TimeSpan totalBattleTimeSpan = DateTime.Now - healTime;
                    if (totalBattleTimeSpan.TotalSeconds < 480)
                    {
                        double sleepTime = 480 - totalBattleTimeSpan.TotalSeconds + 5;
                        Thread.Sleep((int)sleepTime * 1000);
                    }
                }
            }*/
        }
        /// <summary>
        /// 要塞的战斗
        /// </summary>
        /// <returns></returns>
        public void BattleAtHome()
        {
            /*
            bool wait = false;
            while (true)
            {

                Console.WriteLine("开始下一场战斗 @{0}", DateTime.Now);
                Thread.Sleep(rnd.Next(800, 1000));

                HealByNpc();
                //heal
                DateTime healTime = DateTime.Now;
                if (healTime > new DateTime(2018, 8, 8, 4, 50, 0, DateTimeKind.Local))
                {
                    Console.Read();
                    Console.WriteLine("退出");
                    return 0;
                }
                for (int i = 0; i < 3; i++)
                {
                    BattleWithPet();

                    //start
                    mre.Set();
                    Console.WriteLine($"({battleCount})进入战斗界面");
                    DateTime startBattleTime = DateTime.Now;
                    while (true)
                    {
                        if (!IsBattlingMode())
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }

                    //wait
                    mre.Reset();
                    string RoundStatus = $"({battleCount})离开战斗界面，Round#{i + 1} 一共花费了" + (DateTime.Now - startBattleTime).TotalSeconds + "s";
                    Console.WriteLine(RoundStatus);
                    if (wait)
                    {
                        //MessageBox.Show(RoundStatus, "", MessageBoxButtons.OK);
                    }
                    Thread.Sleep(rnd.Next(1000, 1500));
                }
                if (wait)
                {
                    Console.WriteLine("等待恢复中");
                    TimeSpan totalBattleTimeSpan = DateTime.Now - healTime;
                    if (totalBattleTimeSpan.TotalSeconds < 480)
                    {
                        double sleepTime = 480 - totalBattleTimeSpan.TotalSeconds + 5;
                        Thread.Sleep((int)sleepTime * 1000);
                    }
                }
            }*/
        }
    }
}
