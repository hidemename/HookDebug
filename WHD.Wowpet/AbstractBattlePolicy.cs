using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WHD.Core;

namespace WHD.Wowpet
{
    /// <summary>
    /// 战斗程序健壮性
    /// </summary>
    public enum BattleRobustness
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 失败一次
        /// </summary>
        FailOnce = 1,
        /// <summary>
        /// 失败二次
        /// </summary>
        FailTwice = 2,
        /// <summary>
        /// 全部失败
        /// </summary>
        FailAllNeedWait = 3,
    }
    public abstract class AbstractBattlePolicy
    {
        protected int MaxTurn { get; set; }
        protected int turn { get; set; }
        protected DateTime EndTime { get; set; }
        protected int TotalBattleCount { get; set; }
        protected int ErrorWhenEnemyTrackingRetry { get; set; }
        protected int InvaildMinBattleTime = 10; 
        public bool skillHealUsed = false;
        public BattleRobustness battleRobustness = BattleRobustness.Normal;
        public DateTime LastTryToHealDueToRobustnessTime = DateTime.Now;
        protected WowProcess process;
        //public AbstractBattlePolicy(int turn, DateTime endTime)
        //{
        //    Turn = turn;
        //    EndTime = endTime;
        //}

        public abstract void BeforeEnemyTracking();
        public abstract void EnemyTrackingAction();
        public abstract void AfterEnemyTracking();
        public abstract void BattleFinished();
        public abstract void AllTurnFinished();
        public abstract void TryToHealDueToRobustness();
        public int Run()
        {
            ConsoleAndLogInfo($"BattleStart:{MaxTurn}");
            
            while (true)
            {
                for (turn = 1; turn <= MaxTurn; turn++)
                {
                    if (DateTime.Now > EndTime)
                    {
                        ConsoleAndLogInfo($"到了结束时间，退出");
                        Console.Read();
                        return 0;
                    }

                    // EnemyTracking
                    ConsoleAndLogInfo($"({turn})EnemyTracking");
                    BeforeEnemyTracking();
                    var EnemyTrackingRetry = 0;
                    while (true)
                    {
                        EnemyTrackingRetry++;

                        EnemyTrackingAction();

                        if (process.IsBattlingMode())
                        {
                            break;
                        }
                        else
                        {
                            if (skillHealUsed && process.PetStatus.CheckIfCanHeal() && (DateTime.Now - LastTryToHealDueToRobustnessTime).TotalSeconds > 500)
                            {
                                SkillHealPet();
                                ConsoleAndLogInfo($"重新开始回合计数");
                                battleRobustness = BattleRobustness.Normal;
                                turn = 1;
                                process.PetStatus.ResetHealTime();
                            }
                        }

                        if (EnemyTrackingRetry > ErrorWhenEnemyTrackingRetry)
                        {
                            ConsoleAndLogInfo($"无法进入战斗{ErrorWhenEnemyTrackingRetry}次，退出");
                            Console.Read();
                            return 0;
                        }
                    }
                    AfterEnemyTracking();


                    //Battling
                    ConsoleAndLogInfo($"({turn})BattlingMode");
                    //start
                    process.mre.Set();
                    DateTime startBattleTime = DateTime.Now;
                    while (true)
                    {
                        if (!process.IsBattlingMode())
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    //wait
                    process.mre.Reset();

                    //BattleFinished
                    double TotalSeconds = (DateTime.Now - startBattleTime).TotalSeconds;
                    string RoundStatus = $"BattleFinished,Round#{turn}:\t\t{ Convert.ToDecimal(TotalSeconds),2}s";
                    ConsoleAndLogInfo(RoundStatus);
                    if (TotalSeconds < InvaildMinBattleTime)
                    {
                        //健壮性降低
                        battleRobustness += 1;
                        ConsoleAndLogInfo($"!!!健壮性：({battleRobustness})");
                        turn--;
                    }
                    else
                    {
                        if (battleRobustness == BattleRobustness.FailTwice)
                        {
                            ConsoleAndLogInfo($"上次的尝试治疗成功");
                            turn = 1;
                            process.PetStatus.HealTime = LastTryToHealDueToRobustnessTime;
                        }
                        battleRobustness = BattleRobustness.Normal;
                        TotalBattleCount++;
                        ConsoleAndLogInfo($"({TotalBattleCount}):TotalBattleCount");
                    }
                    if (battleRobustness == BattleRobustness.FailTwice)
                    {
                        TryToHealDueToRobustness();
                        LastTryToHealDueToRobustnessTime = DateTime.Now;
                        skillHealUsed = true;
                    }
                    if (battleRobustness == BattleRobustness.FailAllNeedWait)
                    {
                        turn = MaxTurn;
                    }
                    BattleFinished();
                    Thread.Sleep(2000);
                }
                ConsoleAndLogInfo("AllTurnFinished");
                if (process.PetStatus.CheckIfCanHeal())
                {
                    SkillHealPet();
                    battleRobustness = BattleRobustness.Normal;
                    process.PetStatus.ResetHealTime();
                }
                else
                {
                    //if (process.useBandage)
                    //{
                    //    BandageHealPet();
                    //    ConsoleAndLogInfo("使用绷带");
                    //    continue;
                    //}
                    //else
                    //{
                        double sleepTime = 500 - process.PetStatus.GetLastHealSeconds() + 5;
                        Thread.Sleep((int)sleepTime * 1000);
                        ConsoleAndLogInfo("等待恢复CD完成");
                        SkillHealPet();
                        battleRobustness = BattleRobustness.Normal;
                        process.PetStatus.ResetHealTime();
                    //}
                }
            }
        }

        public abstract void Battle();


        public void ConsoleAndLogInfo(string s)
        {
            Console.WriteLine(s);
            LogHelper.Info(s);
        }

        private void HealByNpc()
        {
            Console.Write("治疗中...");
            //process.NatualSendKey(Keys.OemOpenBrackets);
            process.RandomSleep(200, 500);
            process.NatualSendKey(Environment.KeyForwardTalk);
            process.RandomSleep(5000, 5300);
            //process.NatualSendKey(Keys.OemOpenBrackets);
            process.RandomSleep(500, 1000);
            Console.WriteLine("治疗完成");

            SkillHealPet();
        }
        private void BattleWithHomePet(int x = 964, int y = 478)
        {
            //Console.Write($"({battleCount})寻找木桩战斗...");
            process.NatualSendMouse(true, x, y);
            process.RandomSleep(3000, 3500);
            Console.WriteLine("结束");
        }
        protected void SkillHealPet()
        {
            ConsoleAndLogInfo("治疗宠物");
            process.RandomSleep(500, 550);
            process.NatualSendKey(Environment.SkillHealPet);
            process.RandomSleep(500, 550);
        }
        private void BandageHealPet()
        {
            process.RandomSleep(500, 550);
            process.NatualSendKey(Environment.BandageHealPet);
            process.RandomSleep(500, 550);
        }

        private void BattleWithPet()
        {
            //Console.Write($"({battleCount})寻找宠物战斗...");
            //process.NatualSendKey(Keys.OemCloseBrackets);
            process.RandomSleep(200, 500);
            //process.NatualSendKey(_keyAutoMoveAndTalk);
            process.RandomSleep(5000, 5300);
            Console.WriteLine("结束");
        }
    }
}
