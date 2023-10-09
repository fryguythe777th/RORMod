using Microsoft.Xna.Framework;
using RiskOfTerrain.Items.Accessories.Aspects;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class BlazingElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.RedDye);

        public Vector2 blazeSpotPrev;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
            if (active)
            {
                if (blazeSpotPrev == Vector2.Zero)
                    blazeSpotPrev = npc.position;

                var diff = npc.position - blazeSpotPrev;
                float distance = diff.Length().UnNaN();
                if (Main.netMode != NetmodeID.MultiplayerClient && (Main.GameUpdateCount % 40 == 0 || distance > 40f))
                {
                    if (npc.realLife < 0)
                    {
                        var v = new Vector2(npc.position.X + npc.width / 2f, npc.position.Y + npc.height - 10f);
                        for (int i = 0; i <= (int)(distance / 50f); i++)
                        {
                            var p = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), v + Vector2.Normalize(-diff).UnNaN() * 50f * i,
                                Main.rand.NextVector2Unit() * 0.2f, ProjectileID.GreekFire1 + Main.rand.Next(3), 10, 1f, Main.myPlayer, 1f, 1f);

                            p.timeLeft /= 3;
                            p.ROR().spawnedFromElite = true;
                            if (npc.friendly)
                            {
                                p.hostile = false;
                                p.friendly = true;
                            }
                        }
                    }
                    blazeSpotPrev = npc.position;
                }
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (active)
            {
                if (Main.hardMode)
                {
                    target.AddBuff(BuffID.OnFire3, 120);
                }
                else
                {
                    target.AddBuff(BuffID.OnFire, 120);
                }
            }
        }

        public override bool CanRoll(NPC npc)
        {
            return !ServerConfig.Instance.BlazingElitesDisable;
        }

        public override void OnKill(NPC npc)
        {
            if (active)
            {
                int rollNumber = npc.boss ? 1000 : 4000;
                if (Main.player[Player.FindClosest(npc.Center, 500, 500)].RollLuck(rollNumber) == 0)
                {
                    int i = Item.NewItem(npc.GetSource_GiftOrReward(), npc.Center, ModContent.ItemType<BlazingAspect>());
                    Main.item[i].velocity = new Vector2(0, -4);
                }
            }
        }
    }
}