using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Graphics.Primitives;
using RiskOfTerrain.Items.Accessories.T1Common;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Utility
{
    public class MonsterToothProj : ModProjectile
    {
        public TrailRenderer prim;
        public int blinkCounter = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if ((int)Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(RiskOfTerrain.GetSound("monstertoothspawn", 0.2f), Projectile.Center);
            }

            int grabRange = 200;
            if (Main.player[Projectile.owner].lifeMagnet)
            {
                grabRange += Item.lifeGrabRange;
            }

            int closestPlr = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead)
                {
                    var plrHitbox = Main.player[i].Hitbox;
                    if (Projectile.Hitbox.Intersects(plrHitbox))
                    {
                        Main.player[i].Heal(8 + (int)(Main.player[i].statLifeMax * (0.02 * Projectile.GetParentHandler().GetItemStack(ModContent.ItemType<MonsterTooth>()))));
                        SoundEngine.PlaySound(RiskOfTerrain.GetSound("monstertoothheal", 0.1f), Projectile.Center);
                        Projectile.Kill();
                        return;
                    }
                    else if (Projectile.timeLeft < 500)
                    {
                        float distance = Projectile.Distance(Main.player[i].Center);
                        if (distance < grabRange)
                        {
                            grabRange = (int)distance;
                            closestPlr = i;
                        }
                    }
                }
            }

            if (closestPlr != -1)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.player[closestPlr].Center - Projectile.Center) * 20f, 0.2f);
                Projectile.timeLeft = Math.Max(Projectile.timeLeft, 200);
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.velocity.X *= 0.99f;
                Projectile.velocity.Y += 0.3f;
                Projectile.tileCollide = true;
            }

            Projectile.rotation += Projectile.velocity.X * 0.02f;
            if (Projectile.timeLeft < 180)
            {
                if (blinkCounter >= 2 + Projectile.timeLeft / 30)
                {
                    blinkCounter = 0;
                    if (Projectile.alpha > 0)
                    {
                        Projectile.alpha = 0;
                    }
                    else
                    {
                        Projectile.alpha = 255;
                    }
                }

                blinkCounter++;
            }

            Projectile.CollideWithOthers();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X && Math.Abs(Projectile.velocity.X) > 0.5f)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.X *= 0.8f;
                if (Math.Abs(Projectile.velocity.Y) > 2f)
                    Projectile.velocity.Y = -oldVelocity.Y * 0.2f;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (prim == null)
            {
                prim = new TrailRenderer(TextureAssets.Extra[197].Value, TrailRenderer.DefaultPass, (p) => new Vector2(16f) * (1f - p), (p) => new Color(70, 255, 70, 100), drawOffset: Projectile.Size / 2f);
            }
            prim.Draw(Projectile.oldPos);
            var texture = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}