using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WHD.Wowpet
{
    /// <summary>
    /// 野外宠物NPC对战策略。
    /// </summary>
    public class WildPetBattlePolicy : AbstractBattlePolicy
    {
        public WildPetBattlePolicy(WowProcess process,int maxTurn):base(process,maxTurn,
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
            Console.Write($"寻找野宠战斗...");
            process.NatualSendKey(Environment.KeySelectWildPet);
            process.RandomSleep(200, 500);
            process.NatualSendKey(Environment.KeyForwardTalk);
            process.RandomSleep(5000, 5300);
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
