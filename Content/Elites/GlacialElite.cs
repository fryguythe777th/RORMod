using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Projectiles.Elite;
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

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (active)
            {
                target.AddBuff(ModContent.BuffType<GlacialSlow>(), 300);
            }
        }

        public override bool PreKill(NPC npc)
        {
            if (active)
            {
                Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ModContent.ProjectileType<GlacialBomb>(), 0, 0, ai0: 0);
            }
            return true;
        }

        public override bool CanRoll(NPC npc)
        {
            return true;
        }
    }
}