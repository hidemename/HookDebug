using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WHD.Wowpet
{
    public class Environment
    {
        /// <summary>
        /// 自动战斗"\"
        /// </summary>
        public const Keys KeyAutoBattle = Keys.OemBackslash;
        /// <summary>
        /// 自动接近并交互"\"
        /// </summary>
        public const Keys KeyForwardTalk = Keys.OemBackslash;
        /// <summary>
        /// 找到对手NPC并且点击对话框的第一个选项宏"["
        /// </summary>
        public const Keys KeySelectEnemyNpcAndSelectGossipOption1 = Keys.OemOpenBrackets;
        /// <summary>
        /// 使用技能治疗宠物"'"
        /// </summary>
        public const Keys SkillHealPet = Keys.OemQuotes;
        /// <summary>
        /// 使用绷带治疗宠物";"
        /// </summary>
        public const Keys BandageHealPet = Keys.OemSemicolon;
        /// <summary>
        /// 找到要塞治疗宠物的NPCC并且点击对话框的第一个选项宏"]"
        /// </summary>
        public const Keys NPCHealPetAndSelectGossipOption1 = Keys.OemCloseBrackets;
    }
}
