using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHD.Wowpet
{
    public class FortressNpcBattlePolicy : FortressBattlePolicy
    {
        public FortressNpcBattlePolicy(WowProcess process, int maxTurn) : base(process, maxTurn)
        {
        }

        public override void EnemyTrackingAction()
        {
            Console.Write($"寻找NPC战斗...");
            process.NatualSendKey(Environment.KeySelectEnemyNpcAndSelectGossipOption1);
            process.RandomSleep(200, 500);
            process.NatualSendKey(Environment.KeyForwardTalk);
            process.RandomSleep(2500, 3000);
            process.NatualSendKey(Environment.KeySelectEnemyNpcAndSelectGossipOption1);
            process.RandomSleep(500, 1000);
            Console.WriteLine("结束");
        }
    }
}
