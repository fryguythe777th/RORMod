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

        [Name("RORHealthbar")]
        [Desc("RORHealthbar")]
        [MemberBGColor]
        [DefaultValue(typeof(ROR2HealthBar.State), "Enabled")]
        public ROR2HealthBar.State RORHealthbar;

        [Name("RORHealthbarDrawBuff")]
        [Desc("RORHealthbarDrawBuff")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool RORHealthbarDrawBuff;

        [Name("PlayerHealthbarOverlay")]
        [Desc("PlayerHealthbarOverlay")]
        [MemberBGColor]
        [DefaultValue(true)]
        public bool PlayerHealthbarOverlay;
    }
}
