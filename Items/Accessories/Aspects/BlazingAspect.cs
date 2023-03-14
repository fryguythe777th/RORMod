using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace RiskOfTerrain.Items.Accessories.Aspects
{
    public class BlazingAspect : GenericAspect
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }

        public Vector2 blazeSpotPrev;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (blazeSpotPrev == Vector2.Zero)
                blazeSpotPrev = player.position;

            var diff = player.position - blazeSpotPrev;
            float distance = diff.Length().UnNaN();
            if (Main.netMode != NetmodeID.MultiplayerClient && (Main.GameUpdateCount % 40 == 0 || distance > 40f))
            {
                var v = new Vector2(player.position.X + player.width / 2f, player.position.Y + player.height - 10f);
                for (int i = 0; i <= (int)(distance / 50f); i++)
                {
                    var p = Projectile.NewProjectileDirect(player.GetSource_FromAI(), v + Vector2.Normalize(-diff).UnNaN() * 50f * i,
                        Main.rand.NextVector2Unit() * 0.2f, ProjectileID.GreekFire1 + Main.rand.Next(3), 10, 1f, Main.myPlayer, 1f, 1f);

                    p.timeLeft /= 3;
                    p.ROR().spawnedFromElite = true;
                    p.hostile = false;
                    p.friendly = true;
                }
                blazeSpotPrev = player.position;
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, TorchID.Orange);
        }
    }
}