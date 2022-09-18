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

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
            if (active)
            {
                var diff = npc.position - npc.oldPosition;
                if (npc.oldPosition == Vector2.Zero)
                    diff = Vector2.One;
                float distance = diff.Length().UnNaN();
                if (Main.netMode != NetmodeID.MultiplayerClient && (Main.GameUpdateCount % Math.Max((int)(40 - npc.velocity.Length() * 2f), 1) == 0 || distance > 20f))
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
            }
        }
    }
}