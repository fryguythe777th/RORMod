using RORMod.Common;
using RORMod.Content;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RORMod
{
    public class ClientConfig : ConfigurationBase
    {
        public static ClientConfig Instance;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Name("ScreenShake")]
        [Desc("ScreenShake")]
        [MemberBGColor]
        [Range(0f, 1f)]
        [DefaultValue(1f)]
        public float ScreenShake;

        [Name("BossHealthbarActive")]
        [Desc("BossHealthbarActive")]
        [MemberBGColor]
        [DefaultValue(typeof(ROR2BossHealthBar.State), "Enabled")]
        public ROR2BossHealthBar.State BossHealthbarActive;

        [Name("BossHealthbarBottom")]
        [Desc("BossHealthbarBottom")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool BossHealthbarBottom;

        [Name("EnemyHB")]
        [Desc("EnemyHB")]
        [MemberBGColor]
        [DefaultValue(typeof(EnemyHealthBar.State), nameof(EnemyHealthBar.State.RORTheme))]
        public EnemyHealthBar.State EnemyHBState;

        [Name("NPCDrawBuff")]
        [Desc("NPCDrawBuff")]
        [MemberBGColor]
        [DefaultValue(false)]
        public bool NPCDrawBuff;

        [Name("NPCDrawBuff_All")]
        [Desc("NPCDrawBuff_All")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool NPCDrawBuff_All;

        [Name("PlayerHealthbarOverlay")]
        [Desc("PlayerHealthbarOverlay")]
        [MemberBGColor]
        [DefaultValue(true)]
        public bool PlayerHealthbarOverlay;
    }
}
