using Microsoft.Xna.Framework;
using RiskOfTerrain.Items.Accessories.T1Common;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.Void
{
    public class WeepingFungus : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.VoidTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 5);
        }

        public int wungCounter = 60;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ROR().Sprinting)
            {
                if (wungCounter > 0)
                {
                    wungCounter--;
                }
                else
                {
                    wungCounter = 60;
                    player.Heal((int)(player.statLifeMax2 * 0.02));
                }

                if (wungCounter % 5 == 0)
                {
                    Dust.NewDust(player.Center, 2, 2, ModContent.DustType<WungusDust>(), Main.rand.Next(-10, 11) / 10, -2);
                }
            }
            else
            {
                wungCounter = 60;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BustlingFungus>())
                .AddCondition(Condition.NearShimmer)
                .Register();
        }
    }

    public class WungusDust : ModDust
    {
        public float initialVelX = 0;
        public float initialPosX = 0;

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 0;
            dust.noLight = true;
            dust.noGravity = true;
            initialVelX = dust.velocity.X;
            initialPosX = dust.position.X;
        }

        public int timer = 0;

        //public override bool Update(Dust dust)
        //{
        //    timer++;

        //    //dust.alpha = timer;

        //    if (timer == 255)
        //    {
        //        dust.active = false;
        //    }

        //    const float oneRevolutionPerSecond = 6 * 60;
        //    float revolutionsPerSecond = 0.5f;
        //    float sin = (float)Math.Sin(MathHelper.ToRadians(timer * revolutionsPerSecond * oneRevolutionPerSecond));

        //    dust.velocity.X = initialVelX + (sin * 2f);
        //    dust.velocity.Y = -1f;

        //    dust.position.X = initialPosX + (sin * 2f);
        //    dust.position.Y -= 1f;

        //    return false;
        //}
    }
}