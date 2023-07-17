using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.WakeOfVultures
{
    public abstract class BasicWOVBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }
    }

    public class BlazingWOV : BasicWOVBuff
    {
    }

    public class CelestineWOV : BasicWOVBuff
    {
    }

    public class MendingWOV : BasicWOVBuff
    {
    }

    public class OverloadingWOV : BasicWOVBuff
    {
    }
}