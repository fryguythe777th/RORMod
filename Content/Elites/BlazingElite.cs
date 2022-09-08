using Microsoft.Xna.Framework;
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
                if (Main.netMode != NetmodeID.MultiplayerClient && Main.GameUpdateCount % 10 == 0)
                {
                    var p = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), new Vector2(npc.position.X + npc.width / 2f, npc.position.Y + npc.height - 10f),
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