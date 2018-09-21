using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHD.Wowpet
{
    /// <summary>
    /// 要塞里面和对战木桩策略
    /// </summary>
    public class FortressStakesBattlePolicy : FortressBattlePolicy
    {
        public FortressStakesBattlePolicy(WowProcess process, int maxTurn) : base(process, maxTurn)
        {
            throw new NotImplementedException();
        }

        public override void EnemyTrackingAction()
        {
            throw new NotImplementedException();
            //BattleWithStakes(964, 478);
        }
        private void BattleWithStakes(int x, int y)
        {
            Console.Write($"要塞木桩的战斗...");
            process.NatualSendMouse(true, x, y);
            process.RandomSleep(3000, 3500);
            Console.WriteLine("结束");
        }
    }
}
