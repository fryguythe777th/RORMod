using RiskOfTerrain.Content;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.Debuff
{
    public class ArmorEffectCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
}