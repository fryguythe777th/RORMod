using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class RollOfPennies : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void OnHitBy(EntityInfo entity, EntityInfo attacker, Player.HurtInfo info)
        {
            if (entity.entity is not Player && (!Main.expertMode || entity.entity is not NPC))
                return;

            int[] coins = Utils.CoinsSplit(50 * info.Damage);
            var source = entity.entity.GetSource_FromThis();
            var loc = entity.entity.Center;
            if (coins[0] > 0)
                Item.NewItem(source, loc, ItemID.CopperCoin, coins[0]);

            if (coins[1] > 0)
                Item.NewItem(source, loc, ItemID.SilverCoin, coins[1]);

            if (coins[2] > 0)
                Item.NewItem(source, loc, ItemID.GoldCoin, coins[2]);

            if (coins[3] > 0)
                Item.NewItem(source, loc, ItemID.PlatinumCoin, coins[3]);
        }
    }
}