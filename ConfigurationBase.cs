using Terraria.ModLoader.Config;

namespace ROR2Artifacts
{
    [BackgroundColor(10, 10, 40, 220)]
    public abstract class ConfigurationBase : ModConfig
    {
        protected const string Key = "$Mods.ROR2Artifacts.Configuration.";

        protected class NameAttribute : LabelAttribute
        {
            public NameAttribute(string name) : base(Key + name)
            {
            }
        }
        protected class DescAttribute : TooltipAttribute
        {
            public DescAttribute(string tooltip) : base(Key + tooltip + "Tooltip")
            {
            }
        }
        protected class MemberBGColorAttribute : BackgroundColorAttribute
        {
            public MemberBGColorAttribute() : base(47, 29, 140, 180)
            {
            }
        }
        protected class MemberBGColor_SecondaryAttribute : BackgroundColorAttribute
        {
            public MemberBGColor_SecondaryAttribute() : base(80, 80, 130, 180)
            {
            }
        }
    }
}