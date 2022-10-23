using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs
{
    public class MedkitBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 1)
            {
                player.Heal(10 + (int)(player.statLifeMax2 * (0.05f * player.ROR().Accessories.GetItemStack(ModContent.ItemType<Medkit>()))));
                SoundEngine.PlaySound(RiskOfTerrain.GetSound("medkit", 0.1f));
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false; // Prevents removing manually
        }
    }
}