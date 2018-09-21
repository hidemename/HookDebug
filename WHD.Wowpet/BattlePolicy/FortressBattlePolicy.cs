using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHD.Wowpet
{
    /// <summary>
    /// 要塞里面的宠物对战策略父类
    /// </summary>
    public abstract class FortressBattlePolicy : AbstractBattlePolicy
    {
        private static DateTime TaskResetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 00, 00);
        public FortressBattlePolicy(WowProcess process, int maxTurn):base(process,maxTurn, 
            DateTime.Now> TaskResetTime? TaskResetTime.AddDays(1): TaskResetTime, 20)
        {
        }
        public override void AfterEnemyTracking()
        {
        }

        public override void AllTurnFinished()
        {
            NPCHealPet();
        }
        
        public override void BattleFinished()
        {
        }

        public override void BeforeEnemyTracking()
        {
        }
        public override void TryToHealDueToRobustness()
        {
            NPCHealPet();
        }
        public override void HealPet()
        {
            NPCHealPet();
        }
        private void NPCHealPet()
        {
            ConsoleAndLogInfo("NPC治疗宠物");
            process.NatualSendKey(Environment.NPCHealPetAndSelectGossipOption1);
            process.RandomSleep(200, 500);
            process.NatualSendKey(Environment.KeyForwardTalk);
            process.RandomSleep(3000, 5000);
            process.NatualSendKey(Environment.NPCHealPetAndSelectGossipOption1);
            process.RandomSleep(500, 1000);

            turn = 1;
            battleRobustness = BattleRobustness.Normal;
            process.PetStatus.ResetHealTime();
        }
        public override void BattleRobustnessLow(BattleRobustness battleRobustness)
        {
            NPCHealPet();
        }
    }
}
