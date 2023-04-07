using RiskOfTerrain.Content.Accessories;
using System;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class Aegis : ModAccessory
    {
        public int lifeCheck;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var ror = player.ROR();
            ror.accAegis = true;
            ror.barrierMinimumFrac = 0.1f;
        }

        public override void OnEquip(EntityInfo entity)
        {
        }

        public override void OnUnequip(EntityInfo entity)
        {
            if (entity.entity is Player player)
            {
                player.ROR().barrierLife = 0;
            }
        }

        public override void PostUpdate(EntityInfo entity)
        {
            if (entity.entity is Player player)
            {
                var ror = player.ROR();

                if (lifeCheck <= 0)
                    lifeCheck = player.statLife;
                lifeCheck = Math.Min(lifeCheck, player.statLife);

                if (lifeCheck / 2 < player.statLife / 2)
                {
                    ror.barrierLife += player.statLife / 2 - lifeCheck / 2;
                    if (ror.barrierLife > player.statLifeMax2)
                        ror.barrierLife = player.statLifeMax2;
                    lifeCheck = player.statLife / 2 * 2;
                }

                if (player.statLife == player.statLifeMax2 && ror.barrierLife < ror.BarrierMinimum)
                {
                    if (Main.GameUpdateCount % 4 == 0)
                    {
                        ror.barrierLife++;
                        player.statLife++;
                        lifeCheck = player.statLife;
                    }
                }
            }
        }
    }
}
