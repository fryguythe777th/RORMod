using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class HuntersHarpoonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public static void AddStack(Entity entity, int time, int stacksAmt)
        {
            if (entity is Player player && stacksAmt < 5)
                player.AddBuff(ModContent.BuffType<HuntersHarpoonBuff>(), time * stacksAmt);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.accRunSpeed *= 1.8f;
        }
    }
}