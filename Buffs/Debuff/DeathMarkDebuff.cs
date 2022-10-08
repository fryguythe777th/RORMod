using RiskOfTerrain.Content;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.Debuff
{
    public class DeathMarkDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            EnemyHealthBar.BuffIconData.Add(Type, $"{Texture}_Mini");
        }
    }
}