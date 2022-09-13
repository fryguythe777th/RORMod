using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Consumable
{
    public class PowerElixir : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.HealingPotion);
            Item.rare = ItemRarityID.Green;
        }

        public override bool? UseItem(Player player)
        {
            player.potionDelay = (int)(player.potionDelayTime * 1.25f);
            player.AddBuff(BuffID.PotionSickness, player.potionDelay);
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            var ror = player.ROR();
            if (ror.checkElixir == ItemID.None)
                ror.checkElixir = Type;

        }
    }
}