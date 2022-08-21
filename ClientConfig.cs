using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ROR2Artifacts
{
    public class ClientConfig : ConfigurationBase
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public enum HealthbarState
        {
            Disabled,
            Enabled,
            Enabled_AlwaysUse,
        }

        [Name("HealthbarActive")]
        [Desc("HealthbarActive")]
        [MemberBGColor]
        [DefaultValue(typeof(HealthbarState), "Enabled")]
        public HealthbarState HealthbarActive;

        [Name("HealthbarBottom")]
        [Desc("HealthbarBottom")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool HealthbarBottom;
    }
}
