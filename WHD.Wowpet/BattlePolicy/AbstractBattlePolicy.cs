﻿using System;
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
        public AbstractBattlePolicy(WowProcess process, int MaxTurn,DateTime EndTime,int ErrorWhenEnemyTrackingRetry)
        {
            this.process = process;
            this.MaxTurn = MaxTurn;
            this.EndTime = EndTime;
            this.ErrorWhenEnemyTrackingRetry = ErrorWhenEnemyTrackingRetry;
        }
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
        public abstract void TryToHealDueToRobustness();
        public abstract void BattleRobustnessLow(BattleRobustness battleRobustness);
        public virtual void HealPet()
        {
            SkillHealPet();
        }
        public virtual void AllTurnFinished()
        {
            if (process.PetStatus.CheckIfCanHeal())
            {
                HealPet();
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
                ConsoleAndLogInfo("等待恢复CD");
                double sleepTime = 500 - process.PetStatus.GetLastHealSeconds() + 5;
                Thread.Sleep((int)sleepTime * 1000);
                ConsoleAndLogInfo("等待恢复CD完成");
                HealPet();
                battleRobustness = BattleRobustness.Normal;
                process.PetStatus.ResetHealTime();
                //}
            }
        }
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
                                HealPet();
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
                        BattleRobustnessLow(battleRobustness);
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
                AllTurnFinished();
            }
        }


        public void ConsoleAndLogInfo(string s)
        {
            Console.WriteLine(s);
            LogHelper.Info(s);
        }
        
        protected void SkillHealPet()
        {
            ConsoleAndLogInfo("技能治疗宠物");
            process.RandomSleep(500, 550);
            process.NatualSendKey(Environment.SkillHealPet);
            process.RandomSleep(500, 550);
        }
        private void BandageHealPet()
        {
            ConsoleAndLogInfo("绷带治疗宠物");
            process.RandomSleep(500, 550);
            process.NatualSendKey(Environment.BandageHealPet);
            process.RandomSleep(500, 550);
        }

    }
}
