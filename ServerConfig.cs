using RiskOfTerrain.Common;
using RiskOfTerrain.Content;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RiskOfTerrain
{
    public class ServerConfig : ConfigurationBase
    {
        public static ServerConfig Instance;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Name("BlazingElitesDisable")]
        [Desc("BlazingElitesDisable")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool BlazingElitesDisable;

        [Name("CelestineElitesDisable")]
        [Desc("CelestineElitesDisable")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool CelestineElitesDisable;

        [Name("GlacialElitesDisable")]
        [Desc("GlacialElitesDisable")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool GlacialElitesDisable;

        [Name("MendingElitesDisable")]
        [Desc("MendingElitesDisable")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool MendingElitesDisable;

        [Name("OverloadingElitesDisable")]
        [Desc("OverloadingElitesDisable")]
        [MemberBGColor_Secondary]
        [DefaultValue(false)]
        public bool OverloadingElitesDisable;

        [Name("ChestSpawnFrequency")]
        [Desc("ChestSpawnFrequency")]
        [MemberBGColor]
        [Range(0f, 3f)]
        [DefaultValue(1f)]
        public float ChestSpawnFrequency;
    }
}
