using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.OnHitEffects
{
    public class OnHitEffect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.damage = 0;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.knockBack = 0;
            Projectile.alpha = 155;
        }

        

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = (int)Projectile.ai[2] * 4;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= (4 * Projectile.ai[2]) + 4)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(Projectile.whoAmI);
        }
    }

    public class OnHitEffectSpawn : ModSystem
    {
        public static void NewOnHitEffect(Entity spawner, Entity victim, Entity? proj, float variant)
        {
            int variationCoeff = 1;
            Entity where = victim;
            if (proj != null && proj is NPC npc)
            {
                variationCoeff = 2;
                where = proj;
            }
            Projectile.NewProjectile(spawner.GetSource_FromThis(), where.Center + new Vector2(Main.rand.Next(-5 * variationCoeff, (5 * variationCoeff) + 1), Main.rand.Next(-5 * variationCoeff, (5 * variationCoeff) + 1)), Vector2.Zero, ModContent.ProjectileType<OnHitEffect>(), 0, 0, ai2: variant);
        }
    }
}