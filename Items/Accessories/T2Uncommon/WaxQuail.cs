using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class WaxQuail : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => NPC.downedBoss2));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accWaxQuail = true;
        }
    }
}