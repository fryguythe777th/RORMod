using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RORMod.Graphics.Primitives;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class GhorsTomeProj : ModProjectile
    {
        public int blinkCounter = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
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
                Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(0, 360));
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
                    var plr = Main.player[i];
                    if (Projectile.Hitbox.Intersects(plrHitbox))
                    {
                        int[] coins = Utils.CoinsSplit((long)Projectile.ai[1]);

                        if (coins[0] > 0)
                            Item.NewItem(plr.GetSource_FromThis(), plr.Center, ItemID.CopperCoin, coins[0]);

                        if (coins[1] > 0)
                            Item.NewItem(plr.GetSource_FromThis(), plr.Center, ItemID.SilverCoin, coins[1]);

                        if (coins[2] > 0)
                            Item.NewItem(plr.GetSource_FromThis(), plr.Center, ItemID.GoldCoin, coins[2]);

                        if (coins[3] > 0)
                            Item.NewItem(plr.GetSource_FromThis(), plr.Center, ItemID.PlatinumCoin, coins[3]);

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
            return false;
        }
    }
}