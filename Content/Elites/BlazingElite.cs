using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace RORMod.Content.Elites
{
    public class BlazingElite : EliteNPC
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
                if (Main.netMode != NetmodeID.MultiplayerClient && (Main.GameUpdateCount % 40 == 0 || distance > 20f))
                {
                    if (npc.realLife < 0)
                    {
                        var v = new Vector2(npc.position.X + npc.width / 2f, npc.position.Y + npc.height - 10f);
                        for (int i = 0; i <= (int)(distance / 30f); i++)
                        {
                            var p = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), v + Vector2.Normalize(-diff).UnNaN() * 30f * i,
                                Main.rand.NextVector2Unit() * 0.2f, ProjectileID.GreekFire1 + Main.rand.Next(3), 10, 1f, Main.myPlayer, 1f, 1f);

                            p.timeLeft /= 2;
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
    }
}