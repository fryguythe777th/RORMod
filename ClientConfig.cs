using RORMod.Common;
using RORMod.Content;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RORMod
{
    public class ClientConfig : ConfigurationBase
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Name("BossHealthbarActive")]
        [Desc("BossHealthbarActive")]
        [MemberBGColor]
        [DefaultValue(typeof(ROR2BossHealthBar.HealthbarState), "Enabled")]
        public ROR2BossHealthBar.HealthbarState BossHealthbarActive;

        [Name("BossHealthbarBottom")]
        [Desc("BossHealthbarBottom")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool BossHealthbarBottom;

        [Name("PlayerHealthbarOverlay")]
        [Desc("PlayerHealthbarOverlay")]
        [MemberBGColor]
        [DefaultValue(true)]
        public bool PlayerHealthbarOverlay;
    }
}
