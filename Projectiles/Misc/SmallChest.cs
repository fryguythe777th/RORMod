using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RORMod.Items;
using RORMod.Items.Consumable;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace RORMod.Projectiles.Misc
{
    public class SmallChest : ModProjectile
    {
        public const int STATE_OPENING = 3;

        public bool hovering;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 16;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 100;
            Projectile.hide = true;
            Projectile.aiStyle = -1;
        }

        public virtual void OpenChest(Player player)
        {
            Projectile.ai[0] = STATE_OPENING;
            Projectile.scale = 1f;
            Projectile.netUpdate = true;
        }

        public virtual int GetCoinPrice()
        {
            int basePrice = Item.buyPrice(silver: 25);
            if (Main.expertMode)
            {
                basePrice *= 2;
            }
            if (Main.hardMode)
            {
                basePrice *= 2;
            }
            if (NPC.downedMoonlord)
            {
                basePrice *= 2;
            }
            int finalPrice = basePrice;
            foreach (var price in RORMod.PriceModifiers)
            {
                if (price.Item1())
                {
                    finalPrice += (int)(basePrice * price.Item2);
                }
            }
            return finalPrice;
        }

        public virtual void UpdateHover(Player player)
        {
            int price = GetCoinPrice();
            if (Main.mouseRight && Main.mouseRightRelease && player.BuyItem(price))
            {
                SoundEngine.PlaySound(SoundID.Unlock.WithPitchOffset(-0.5f), Main.MouseWorld);
                SoundEngine.PlaySound(SoundID.Coins, Main.MouseWorld);
                OpenChest(player);
                return;
            }
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemID.GoldCoin;
        }

        public virtual void DropItems(float proc)
        {
            switch (proc)
            {
                case <= 0.19f:
                    Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), Main.rand.Next(RORItem.GreenTier));
                    break;

                case <= 0.20f:
                    Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), Main.rand.Next(RORItem.RedTier));
                    break;

                default:
                    Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), Main.rand.Next(RORItem.WhiteTier));
                    break;
            }
        }

        public virtual void HandleDissapearingAfterBeingOpened()
        {
            if (Projectile.ai[1] < 240f)
                Projectile.timeLeft = 60;

            Projectile.Opacity = Projectile.timeLeft / 60f;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            if ((int)Projectile.ai[0] == 3)
            {
                if (Projectile.localAI[0] > 2f)
                {
                    Projectile.localAI[0] = 2f;
                }
                else if (Projectile.localAI[0] > 0f)
                {
                    Projectile.localAI[0]--;
                    if (Projectile.localAI[0] < 0f)
                        Projectile.localAI[0] = 0f;
                }
                Projectile.frameCounter++;
                Projectile.ai[1]++;

                if (Projectile.frameCounter > 3)
                {
                    Projectile.frameCounter = 0;
                    if (Projectile.frame < Main.projFrames[Type] - 1)
                        Projectile.frame++;
                }

                HandleDissapearingAfterBeingOpened();

                if ((int)Projectile.ai[1] == 8 && Main.myPlayer == Projectile.owner)
                {
                    float proc = Main.rand.NextFloat();
                    DropItems(proc);
                }
                return;
            }
            if ((int)Projectile.ai[0] == 0)
            {
                Projectile.scale = 0.1f;
                Projectile.rotation = Main.rand.NextFloat(-0.2f, 0.2f);
                Projectile.spriteDirection = Main.player[Projectile.owner].direction;
                Projectile.ai[0] = 1f;
            }

            if (Projectile.extraUpdates == 0 && Projectile.numUpdates == -1)
            {
                Projectile.CollideWithOthers();
                Projectile.velocity.X *= 0.9f;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && Projectile.Distance(Main.player[i].Center) < 2000f)
                    {
                        Projectile.timeLeft = Math.Max(Projectile.timeLeft, 2);
                        break;
                    }
                }

                if ((int)Projectile.ai[0] == 2)
                {
                    Projectile.scale -= 0.01f;
                    if (Projectile.scale < 1f)
                    {
                        Projectile.scale = 1f;
                    }
                }
                else
                {
                    Projectile.scale += 0.1f;
                    if (Projectile.scale > 1.1f)
                    {
                        Projectile.ai[0] = 2f;
                    }
                }
            }
            hovering = false;
            if (Main.netMode != NetmodeID.Server)
            {
                var hitbox = Projectile.getRect();
                hitbox.X -= (int)Main.screenPosition.X;
                hitbox.Y -= (int)Main.screenPosition.Y;
                hitbox.Height += 12;
                var tileCoords = Projectile.Center.ToTileCoordinates();
                var plr = Main.LocalPlayer;
                hovering = !plr.mouseInterface && hitbox.Contains(Main.mouseX, Main.mouseY) && plr.IsInTileInteractionRange(tileCoords.X, tileCoords.Y);
                if (hovering)
                {
                    plr.mouseInterface = true;
                    UpdateHover(plr);
                }

                if (hovering || Projectile.Distance(Main.LocalPlayer.Center) < 72f)
                {
                    Projectile.localAI[0]++;
                }
                else if (Projectile.localAI[0] > 2f)
                {
                    Projectile.localAI[0] = 2f;
                }
                else if (Projectile.localAI[0] > 0f)
                {
                    Projectile.localAI[0]--;
                    if (Projectile.localAI[0] < 0f)
                        Projectile.localAI[0] = 0f;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.extraUpdates = 0;
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.localAI[0] > 0f)
            {
                overWiresUI.Add(index);
                return;
            }
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var frame = Projectile.Frame();
            var origin = new Vector2(frame.Width / 2f, frame.Height - 2);
            var drawCoords = new Vector2(Projectile.position.X + Projectile.width / 2f, Projectile.position.Y + Projectile.height + 10f) - Main.screenPosition;
            var effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()) * Projectile.Opacity;

            if (Projectile.localAI[0] > 0f)
            {
                Main.spriteBatch.End();
                Helpers.BeginShader_GeneralEntities(Main.spriteBatch);
                Helpers.ColorOnlyShader.Apply(Projectile);
                foreach (var c in Helpers.CircularVector(4, Projectile.rotation))
                {
                    Main.spriteBatch.Draw(texture, drawCoords + c * 2f, frame, Main.OurFavoriteColor * 0.8f, Projectile.rotation,
                        origin, Projectile.scale, effects, 0);
                }
                Main.spriteBatch.End();
                Helpers.Begin_GeneralEntities(Main.spriteBatch);
            }

            Main.EntitySpriteDraw(texture, drawCoords, frame, lightColor, Projectile.rotation,
                origin, Projectile.scale, effects, 0);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.ImmediateLoad).Value, drawCoords, frame, Color.White * Projectile.Opacity, Projectile.rotation,
                origin, Projectile.scale, effects, 0);

            if (Projectile.localAI[0] > 0f)
            {
                var scale = new Vector2(1f);
                var font = FontAssets.ItemStack.Value;
                int price = GetCoinPrice();
                var l = new List<string>();
                if (price >= Item.platinum)
                {
                    l.Add($"{price / Item.platinum} platinum");
                    price %= Item.platinum;
                }
                if (price >= Item.gold)
                {
                    l.Add($"{price / Item.gold} gold");
                    price %= Item.gold;
                }
                if (price >= Item.silver)
                {
                    l.Add($"{price / Item.silver} silver");
                    price %= Item.silver;
                }
                if (price >= Item.copper)
                {
                    l.Add($"{price} copper");
                }
                for (int i = 0; i < l.Count; i++)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, l[i], 
                        drawCoords - new Vector2(0f, frame.Height * 1.25f + 20f * (l.Count - i - 1)), Color.Yellow, (Color.Yellow * 0.2f).UseA(100), 0f, font.MeasureString(l[i]) / 2f, Vector2.One);
                }
            }
            return false;
        }

        public void DrawPriceText()
        {

        }
    }
}