using RORMod.Common;
using RORMod.Content;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RORMod
{
    public class ClientConfig : ConfigurationBase
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Name("HealthbarActive")]
        [Desc("HealthbarActive")]
        [MemberBGColor]
        [DefaultValue(typeof(ROR2HealthBar.HealthbarState), "Enabled")]
        public ROR2HealthBar.HealthbarState HealthbarActive;

        [Name("HealthbarBottom")]
        [Desc("HealthbarBottom")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool HealthbarBottom;
    }
}
