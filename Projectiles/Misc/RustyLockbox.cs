using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Items;
using RiskOfTerrain.Items.Consumable;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class RustyLockbox : SmallChest
    {
        public override void UpdateHover(Player player)
        {
            if (Main.mouseRight && Main.mouseRightRelease && player.ConsumeItem(ModContent.ItemType<RustedKey>()))
            {
                SoundEngine.PlaySound(SoundID.Unlock, Main.MouseWorld);
                OpenChest(player);
                return;
            }
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<RustedKey>();
        }

        public override int ChooseItem(float proc)
        {
            switch (proc)
            {
                case >= 0.8f:
                    return Main.rand.Next(RORItem.RedTier);

                default:
                    return Main.rand.Next(RORItem.GreenTier);
            }
        }

        public override void DropItem()
        {
            Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), item);
        }

        public override void HandleDissapearingAfterBeingOpened()
        {
            if (Projectile.ai[1] < 30f)
                Projectile.timeLeft = 20;

            Projectile.Opacity = Projectile.timeLeft / 20f;
        }

        public override void AI()
        {
            base.AI();
            Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.05f, 0f) * (hovering ? 4f : 1f));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var frame = Projectile.Frame();
            var origin = new Vector2(frame.Width / 2f, frame.Height - 2);
            var drawCoords = new Vector2(Projectile.position.X + Projectile.width / 2f, Projectile.position.Y + Projectile.height + 10f) - Main.screenPosition;
            var effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()) * Projectile.Opacity;

            if (Projectile.frame == 0)
            {
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{Texture}Aura", AssetRequestMode.ImmediateLoad).Value, drawCoords - new Vector2(0f, -4f).RotatedBy(Projectile.rotation), null, lightColor * 2f, Projectile.rotation,
                    origin, Projectile.scale, effects, 0);
            }

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
            return false;
        }
    }
}