using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WHD.Wowpet
{
    public class WorldQuestNpcBattlePolicy : AbstractBattlePolicy
    {
        private static DateTime TaskResetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 00, 00);
        public WorldQuestNpcBattlePolicy(WowProcess process, int maxTurn) : base(process, maxTurn,
            DateTime.Now > TaskResetTime ? TaskResetTime.AddDays(1) : TaskResetTime, 20)
        {
        }
        
        public override void AfterEnemyTracking()
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
            process.NatualSendKey(Environment.KeySelectEnemyNpcAndSelectGossipOption1);
            process.RandomSleep(200, 500);
            process.NatualSendKey(Environment.KeyForwardTalk);
            process.RandomSleep(3000, 5000);
            process.NatualSendKey(Environment.KeySelectEnemyNpcAndSelectGossipOption1);
            process.RandomSleep(500, 1000);
            Console.WriteLine("结束");
        }

        public override void TryToHealDueToRobustness()
        {
            HealPet();
        }

        public override void BattleRobustnessLow(BattleRobustness battleRobustness)
        {
        }
    }
}
