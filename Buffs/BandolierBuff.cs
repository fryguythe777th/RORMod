using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class BandolierBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public static void AddStack(Entity entity, int time, int stacksAmt)
        {
            if (entity is Player player)
                player.AddBuff(ModContent.BuffType<BandolierBuff>(), time * stacksAmt);
        }
    }
}