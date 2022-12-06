using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Items.Accessories.T1Common;
using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class FireworksSpawner : ModProjectile
    {
        public override string Texture => RiskOfTerrain.BlankTexture;

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = 12;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 3600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.ai[1]--;
            if (Projectile.ai[1] <= 0f)
            {
                Projectile.ai[1] = 10f;
                Projectile.ai[0]--;
                var parent = Projectile.GetParent();
                if (parent == null)
                {
                    return;
                }
                Projectile.Center = parent.Center;
                var parentHandler = parent.GetHandler();
                var reference = parentHandler.GetItemReference(ModContent.ItemType<BundleOfFireworks>());
                if (new EntityInfo(parent).IsMe())
                {
                    Projectile.NewProjectile(reference != null ? parent.GetSource_Accessory(reference) : null, Projectile.Center + new Vector2(parent.width / 2f * -parent.direction, 0f), new Vector2(0f, -7f).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)),
                        ModContent.ProjectileType<FireworksProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 40f);
                }

                if ((int)Projectile.ai[0] <= -1)
                {
                    Projectile.Kill();
                    return;
                }
            }
            Projectile.timeLeft = 2;
        }
    }
}