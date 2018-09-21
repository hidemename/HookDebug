using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WHD.Wowpet
{
    /// <summary>
    /// 固定的宠物NPC对战策略。比如小艺
    /// </summary>
    public class PetNpcBattlePolicy : AbstractBattlePolicy
    {
        public PetNpcBattlePolicy(WowProcess process,int maxTurn):base(process,maxTurn,
            DateTime.Now.AddMonths(1),20)
        {
        }
        
        public override void AfterEnemyTracking()
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
