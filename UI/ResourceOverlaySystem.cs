using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace RORMod.UI
{
    public class ResourceOverlaySystem : ModSystem
    {
        public static float shield;
        public static float barrier;
        public static float glass;

        public static float MaxShield;
        public static float MaxBarrier;
        public static float MaxGlass;

        public static bool Enabled => ClientConfig.Instance.PlayerHealthbarOverlay;

        public override void Load()
        {
            InitDrawInfo();
            On.Terraria.GameContent.UI.ResourceSets.ClassicPlayerResourcesDisplaySet.DrawLife += ClassicPlayerResourcesDisplaySet_DrawLife;
            On.Terraria.GameContent.UI.ResourceSets.FancyClassicPlayerResourcesDisplaySet.DrawLifeBar += FancyClassicPlayerResourcesDisplaySet_DrawLifeBar;
            On.Terraria.GameContent.UI.ResourceSets.HorizontalBarsPlayerReosurcesDisplaySet.Draw += HorizontalBarsPlayerReosurcesDisplaySet_Draw;
            On.Terraria.GameContent.UI.ResourceSets.PlayerStatsSnapshot.ctor += PlayerStatsSnapshot_ctor;
        }

        private static void PlayerStatsSnapshot_ctor(On.Terraria.GameContent.UI.ResourceSets.PlayerStatsSnapshot.orig_ctor orig, ref Terraria.GameContent.UI.ResourceSets.PlayerStatsSnapshot self, Player player)
        {
            if (!Enabled)
            {
                orig(ref self, player);
                return;
            }
            GlassHP(player, player.ROR().HPLostToGlass);
            orig(ref self, player);
            GlassHP(player, -player.ROR().HPLostToGlass);
        }

        public static void UpdateLerps()
        {
            shield = MathHelper.Lerp(shield, MaxShield, 0.15f);
            barrier = MathHelper.Lerp(barrier, MaxBarrier, 0.15f);
            glass = MathHelper.Lerp(glass, MaxGlass, 0.15f);
            if (MaxGlass == 0f && glass < 0.01f)
                glass = 0f;
            if (MaxBarrier == 0f && barrier < 0.01f)
                barrier = 0f;
            if (MaxShield == 0f && shield < 0.01f)
                shield = 0f;
        }

        public static void GlassHP(Player player, int hp)
        {
            if (glass > 0f && MaxGlass > 0f)
            {
                player.statLife += (int)(hp * (1f - glass / MaxGlass));
            }
            player.statLifeMax2 += hp;
        }

        public static void GlassHP_Classic(Player player, int hp)
        {
            if (glass > 0f && MaxGlass > 0f)
            {
                int amt = (int)(hp * (1f - glass / MaxGlass));
                player.statLife += amt;
                player.statLifeMax2 += amt;
            }
        }

        private static void HorizontalBarsPlayerReosurcesDisplaySet_Draw(On.Terraria.GameContent.UI.ResourceSets.HorizontalBarsPlayerReosurcesDisplaySet.orig_Draw orig, Terraria.GameContent.UI.ResourceSets.HorizontalBarsPlayerReosurcesDisplaySet self)
        {
            if (!Enabled)
            {
                orig(self);
                return;
            }

            UpdateLerps();
            orig(self);

            var frame = new Rectangle(64, 0, 28, 28);

            int x = Main.screenWidth - 300 - 22 + 16;
            int y = 18;
            x -= 2;
            y -= 2;

            float glassLife = 0f;
            if (glass > 0f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/GlassHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/HorizontalBars/HP_Fill").Value;
                glassLife = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(glassLife / 20);
                glassLife *= glass;
                float life = glassLife;
                while (life > 0f)
                {
                    var frame2 = frame;
                    if (life < 20f)
                        frame2.Width = (int)(frame.Width * (life / 20f));
                    Main.spriteBatch.Draw(texture, new Vector2(x + hpTexture.Width * i, y), frame2, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    i++;
                    life -= 20;
                }
            }

            if (shield > 0.01f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/ShieldHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/HorizontalBars/HP_Fill").Value;
                float life = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(life / 20);
                life = Math.Max(life * (shield * (1f - glass)), 8f);
                while (life > 0f)
                {
                    var frame2 = frame;
                    if (life < 20f)
                    {
                        int amt = (int)(frame.Width * (life / 20f));
                        frame2.Width = amt;
                    }
                    Main.spriteBatch.Draw(texture, new Vector2(x - hpTexture.Width * i + hpTexture.Width * 19, y) + new Vector2(28f, 28f), frame2, Color.White * Main.cursorAlpha, MathHelper.Pi, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);
                    i++;
                    life -= 20;
                }
            }

            if (barrier > 0.01f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/BarrierHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/HorizontalBars/HP_Fill").Value;
                float life = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(life / 20);
                life = Math.Max(life * (barrier * (1f - glass)), 5f);
                while (life > 0f)
                {
                    var frame2 = frame;
                    if (life < 20f)
                    {
                        int amt = (int)(frame.Width * (life / 20f));
                        frame2.Width = amt;
                    }
                    Main.spriteBatch.Draw(texture, new Vector2(x + hpTexture.Width * i + hpTexture.Width / 20f * glassLife, y), frame2, new Color(150, 150 - (int)(Math.Sin((Main.GlobalTimeWrappedHourly * 3.5f - i * 0.75f) % Math.PI) * 50f), 125, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    i++;
                    life -= 20;
                }
            }
        }

        private static void FancyClassicPlayerResourcesDisplaySet_DrawLifeBar(On.Terraria.GameContent.UI.ResourceSets.FancyClassicPlayerResourcesDisplaySet.orig_DrawLifeBar orig, Terraria.GameContent.UI.ResourceSets.FancyClassicPlayerResourcesDisplaySet self, SpriteBatch spriteBatch)
        {
            if (!Enabled)
            {
                orig(self, spriteBatch);
                return;
            }

            UpdateLerps();
            orig(self, spriteBatch);

            var frame = new Rectangle(32, 0, 30, 30);

            var drawLoc = new Vector2(Main.screenWidth - 300 + 4, 15f);

            float glassLife = 0f;
            if (glass > 0f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/GlassHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/FancyClassic/Heart_Fill").Value;
                glassLife = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(glassLife / 20);
                glassLife *= glass;

                float life = glassLife;
                while (life > 0f)
                {
                    int heartX = 9 - i % 10;
                    int heartY = 1 - i / 10;
                    float scale = 1f;
                    if (life < 20f)
                        scale = life / 20f;
                    Main.spriteBatch.Draw(texture, new Vector2(drawLoc.X + (hpTexture.Width + 2) * heartX, drawLoc.Y + heartY * 28f) + frame.Size() / 2f, frame, Color.White * scale, 0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
                    i++;
                    life -= 20;
                }
            }

            if (shield > 0.01f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/ShieldHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/FancyClassic/Heart_Fill").Value;
                float life = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = (int)(life / 20) - 1;
                life = Math.Max(life * (shield * (1f - glass)), 8f);
                while (life > 0f)
                {
                    int heartX = 9 - i % 10;
                    int heartY = 1 - i / 10;
                    float scale = 1f;
                    if (life < 20f)
                        scale = life / 20f;
                    Main.spriteBatch.Draw(texture, new Vector2(drawLoc.X + (hpTexture.Width + 2) * heartX, drawLoc.Y + heartY * 28f) + frame.Size() / 2f, frame, Color.White * Main.cursorAlpha, 0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
                    i--;
                    life -= 20;
                }
            }

            if (barrier > 0.01f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/BarrierHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/FancyClassic/Heart_Fill").Value;
                float life = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(life / 20);
                life = Math.Max(life * (barrier * (1f - glass)), 3f);
                i += (int)(glassLife / 20f);
                if (glassLife % 1f > 0.01f)
                    i++;
                while (life > 0f)
                {
                    int heartX = 9 - i % 10;
                    int heartY = 1 - i / 10;
                    float scale = 1f;
                    if (life < 20f)
                        scale = life / 20f;
                    Main.spriteBatch.Draw(texture, new Vector2(drawLoc.X + (hpTexture.Width + 2) * heartX, drawLoc.Y + heartY * 28f) + frame.Size() / 2f, frame, new Color(150, 150 - (int)(Math.Sin((Main.GlobalTimeWrappedHourly * 3.5f - i *0.75f) % Math.PI) * 50f), 125, 100), 0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
                    i++;
                    life -= 20;
                }
            }
        }

        private static void ClassicPlayerResourcesDisplaySet_DrawLife(On.Terraria.GameContent.UI.ResourceSets.ClassicPlayerResourcesDisplaySet.orig_DrawLife orig, Terraria.GameContent.UI.ResourceSets.ClassicPlayerResourcesDisplaySet self)
        {
            if (!Enabled)
            {
                orig(self);
                return;
            }

            UpdateLerps();
            var player = Main.LocalPlayer;
            GlassHP(player, player.ROR().HPLostToGlass);
            orig(self);
            GlassHP(player, -player.ROR().HPLostToGlass);

            var frame = new Rectangle(0, 0, 30, 30);

            var drawLoc = new Vector2(Main.screenWidth - 300 - 4f, 28f);

            float glassLife = 0f;
            if (glass > 0f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/GlassHeart").Value;
                var hpTexture = TextureAssets.Heart.Value;
                glassLife = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(glassLife / 20);
                glassLife *= glass;
                float life = glassLife;
                while (life > 0f)
                {
                    int heartX = 9 - i % 10;
                    int heartY = 1 - i / 10;
                    float scale = 1f;
                    if (life < 20f)
                        scale = life / 20f;
                    Main.spriteBatch.Draw(texture, new Vector2(drawLoc.X + (hpTexture.Width + 4) * heartX, drawLoc.Y + heartY * 26f) + frame.Size() / 2f, frame, Color.White * scale, 0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
                    i++;
                    life -= 20;
                }
            }

            if (shield > 0.01f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/ShieldHeart").Value;
                var hpTexture = TextureAssets.Heart.Value;
                float life = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = (int)(life / 20) - 1;
                life = Math.Max(life * (shield * (1f - glass)), 3f);
                while (life > 0f)
                {
                    int heartX = 9 - i % 10;
                    int heartY = 1 - i / 10;
                    float scale = 1f;
                    if (life < 20f)
                        scale = life / 20f;
                    Main.spriteBatch.Draw(texture, new Vector2(drawLoc.X + (hpTexture.Width + 4) * heartX, drawLoc.Y + heartY * 26f) + frame.Size() / 2f, frame, Color.White * Main.cursorAlpha, 0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
                    i--;
                    life -= 20;
                }
            }

            if (barrier > 0.01f)
            {
                var texture = ModContent.Request<Texture2D>(RORMod.AssetsPath + "UI/BarrierHeart").Value;
                var hpTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PlayerResourceSets/FancyClassic/Heart_Fill").Value;
                float life = Math.Clamp(Main.LocalPlayer.statLifeMax, 0f, 400f);
                int i = 20 - (int)(life / 20);
                life = Math.Max(life * (barrier * (1f - glass)), 3f);
                i += (int)(glassLife / 20f);
                if (glassLife % 1f > 0.01f)
                    i++;
                while (life > 0f)
                {
                    int heartX = 9 - i % 10;
                    int heartY = 1 - i / 10;
                    float scale = 1f;
                    if (life < 20f)
                        scale = life / 20f;
                    Main.spriteBatch.Draw(texture, new Vector2(drawLoc.X + (hpTexture.Width + 4) * heartX, drawLoc.Y + heartY * 26f) + frame.Size() / 2f, frame,
                        new Color(150, 150 - (int)(Math.Sin((Main.GlobalTimeWrappedHourly * 3.5f - i * 0.75f) % Math.PI) * 50f), 125, 100), 0f, frame.Size() / 2f, scale, SpriteEffects.None, 0f);
                    i++;
                    life -= 20;
                }
            }
        }

        public override void Unload()
        {
            InitDrawInfo();
        }

        public override void PreUpdatePlayers()
        {
        }

        public static void InitDrawInfo()
        {
            shield = 0f;
            barrier = 0f;
            glass = 0f;
        }
    }
}