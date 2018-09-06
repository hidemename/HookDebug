using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WHD.Wowpet
{
    public class WorldQuestNpcBattlePolicy : AbstractBattlePolicy
    {
        DateTime TaskResetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 55, 00);
        public WorldQuestNpcBattlePolicy(WowProcess process,int maxTurn)
        {
            base.process = process;
            MaxTurn = maxTurn;
            EndTime = TaskResetTime;
            if (DateTime.Now.Hour >= 7)
            {
                EndTime = EndTime.AddDays(1);
            }
            ErrorWhenEnemyTrackingRetry = 20;
        }
        
        public override void AfterEnemyTracking()
        {
        }

        public override void AllTurnFinished()
        {
        }

        public override void Battle()
        {
        }

        public override void BattleFinished()
        {
        }
        

        public override void BeforeEnemyTracking()
        {
        }

        public override void EnemyTrackingAction()
        {
            Console.Write($"寻找NPC战斗...");
            process.NatualSendKey(Environment.KeySelectEnemyNpc);
            process.RandomSleep(200, 500);
            process.NatualSendKey(Environment.KeyForwardTalk);
            process.RandomSleep(3000, 5000);
            process.NatualSendKey(Environment.KeySelectGossipOption1);
            process.RandomSleep(500, 1000);
            Console.WriteLine("结束");
        }

        public override void TryToHealDueToRobustness()
        {
            SkillHealPet();
        }
        
    }
}
