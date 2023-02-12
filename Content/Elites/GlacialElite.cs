using Microsoft.Xna.Framework;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class GlacialElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.SilverDye);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
        }

        public override void OnKill(NPC npc)
        {
            if (active)
            {
                //int i = Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ModContent.ProjectileType<GlacialEliteProj>(), 0, 0);
                //Main.projectile[i].scale = 0.33f;
                //wip glacial elite blast i was working on
            }
        }

        public override bool CanRoll(NPC npc)
        {
            return false;
        }
    }
}