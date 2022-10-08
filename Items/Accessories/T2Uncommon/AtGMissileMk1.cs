using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class AtGMissileMk1 : StackableAcc
    {
        public virtual int Chance => 10;
        public float statDamageMultiplier;

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
        }

        public override void CalculateStats(Player player, RORPlayer ror)
        {
            ror.accAtG = this;
            statDamageMultiplier = 2f * stack;
        }
    }
}