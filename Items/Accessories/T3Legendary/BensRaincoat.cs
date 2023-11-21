using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using RiskOfTerrain.Buffs;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class BensRaincoat : ModAccessory
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => NPC.downedBoss2));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        int savedBuffCount = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (savedBuffCount < player.CountBuffs())
            {
                if (Main.debuff[player.buffType[savedBuffCount]] && !player.HasBuff(ModContent.BuffType<BensRaincoatBuff>()) && player.buffType[savedBuffCount] != BuffID.PotionSickness)
                {
                    player.DelBuff(savedBuffCount);
                    player.AddBuff(ModContent.BuffType<BensRaincoatBuff>(), 1200);

                    var ror = player.ROR();
                    ror.barrierLife += 50;
                    if (ror.barrierLife > player.statLifeMax2)
                    {
                        ror.barrierLife = player.statLifeMax2;
                    }
                    player.statLife += 50;
                }
            }

            savedBuffCount = player.CountBuffs();
        }
    }
}