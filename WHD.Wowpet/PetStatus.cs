using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHD.Wowpet
{
    public class PetStatus
    {

        public DateTime HealTime { get ; set ; }

        public void ResetHealTime()
        {
            HealTime = DateTime.Now;
        }
        public bool CheckIfCanHeal()
        {
            if (HealTime == null)
            {
                return true;
            }
            var timeSpan = DateTime.Now - HealTime;
            if (timeSpan.TotalSeconds < 500)//8min
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public double GetLastHealSeconds()
        {
            var timeSpan = DateTime.Now - HealTime;
            return timeSpan.TotalSeconds;
        }
    }
}
