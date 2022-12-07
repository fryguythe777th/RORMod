using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class BerzerkerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic) *= 2f;
            player.accRunSpeed *= 1.5f;
        }
    }
}