using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RORMod.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Content
{
    public class EnemyHealthBar : GlobalNPC
    {
        public enum State
        {
            Vanilla,
            RORTheme,
            MixedTheme,
        }

        public static Asset<Texture2D> MixedStyleBar { get; private set; }
        public static Asset<Texture2D> MixedStyleBarFill { get; private set; }

        public static Dictionary<int, string> BuffIconData { get; private set; }
        public static Dictionary<int, Vector2> TopOffset { get; private set; }

        public const int HPBarWidth = 60;

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;
        public int lastLife;
        public int hpBarLife;
        public int hpBarLifeSetDelay;
        private static NPC npcContext;
        private static EnemyHealthBar instanceContext;

        public override void Load()
        {
            MixedStyleBar = ModContent.Request<Texture2D>($"{RORMod.AssetsPath}UI/MixedStyleBar");
            MixedStyleBarFill = ModContent.Request<Texture2D>($"{RORMod.AssetsPath}UI/MixedStyleBar_Fill");
            string t = "RORMod/Buffs/Mini/Buff_";
            BuffIconData = new Dictionary<int, string>()
            {
                [BuffID.Poisoned] = $"{t}{BuffID.Poisoned}",
                [BuffID.OnFire] = $"{t}{BuffID.OnFire}",
                [BuffID.OnFire3] = $"{t}{BuffID.OnFire3}",
                [BuffID.Confused] = $"{t}{BuffID.Confused}",
                [BuffID.CursedInferno] = $"{t}{BuffID.CursedInferno}",
                [BuffID.Frostburn] = $"{t}{BuffID.Frostburn}",
                [BuffID.Frostburn2] = $"{t}{BuffID.Frostburn2}",
                [BuffID.Ichor] = $"{t}{BuffID.Ichor}",
                [BuffID.Venom] = $"{t}{BuffID.Venom}",
                [BuffID.Midas] = $"{t}{BuffID.Midas}",
                [BuffID.Wet] = $"{t}{BuffID.Wet}",
                [BuffID.Stinky] = $"{t}{BuffID.Stinky}",
                [BuffID.Slimed] = $"{t}{BuffID.Slimed}",
                [BuffID.ShadowFlame] = $"{t}{BuffID.ShadowFlame}",
                [BuffID.Oiled] = $"{t}{BuffID.Oiled}",
                [BuffID.Lovestruck] = $"{t}{BuffID.Lovestruck}",
                [BuffID.Bleeding] = $"{t}{BuffID.Bleeding}",
                [BuffID.BoneJavelin] = $"{t}{BuffID.BoneJavelin}",
                [BuffID.StardustMinionBleed] = $"{t}{BuffID.StardustMinionBleed}",
                [BuffID.DryadsWardDebuff] = $"{t}{BuffID.DryadsWardDebuff}",
                [BuffID.BetsysCurse] = $"{t}{BuffID.BetsysCurse}",
                [BuffID.Daybreak] = $"{t}{BuffID.Daybreak}",
                [BuffID.BlandWhipEnemyDebuff] = $"{t}{BuffID.BlandWhipEnemyDebuff}",
                [BuffID.ScytheWhipEnemyDebuff] = $"{t}{BuffID.ScytheWhipEnemyDebuff}",
                [BuffID.FlameWhipEnemyDebuff] = $"{t}{BuffID.FlameWhipEnemyDebuff}",
                [BuffID.RainbowWhipNPCDebuff] = $"{t}{BuffID.RainbowWhipNPCDebuff}",
                [BuffID.GelBalloonBuff] = $"{t}{BuffID.GelBalloonBuff}",
            };
            TopOffset = new Dictionary<int, Vector2>()
            {
                [NPCID.MoonLordHead] = new Vector2(0f, 70f),
                [NPCID.MoonLordHand] = new Vector2(0f, 40f),
            };
            On.Terraria.Main.DrawHealthBar += Main_DrawHealthBar;
        }

        public override void Unload()
        {
            BuffIconData?.Clear();
            BuffIconData = null;
            npcContext = null;
            instanceContext = null;
        }

        public static void DrawRORThemeHB(NPC npc, Vector2 position, float scale)
        {
            var drawCoords = new Point((int)(position.X - Main.screenPosition.X) - (int)(HPBarWidth / 2 * scale), (int)(position.Y - Main.screenPosition.Y));

            if (Main.LocalPlayer.gravDir == -1)
                drawCoords.Y = Main.screenHeight - drawCoords.Y;

            var color = (npcContext.boss || RORNPC.CountsAsBoss.Contains(npcContext.type)) ? Color.Red : Color.Yellow;
            int outSize = (int)Math.Max(2f * scale, 2f) * 2;

            if (npcContext.IsElite())
                Helpers.DrawRectangle(new Rectangle(drawCoords.X - outSize, drawCoords.Y - outSize, (int)(HPBarWidth * scale) + outSize * 2, (int)(4 * scale) + outSize * 2), Color.White);
            Helpers.DrawRectangle(new Rectangle(drawCoords.X - outSize / 2, drawCoords.Y - outSize / 2, (int)(HPBarWidth * scale) + outSize, (int)(4 * scale) + outSize), Color.Black);
            if (instanceContext.hpBarLife > 0)
                Helpers.DrawRectangle(new Rectangle(drawCoords.X, drawCoords.Y, (int)(HPBarWidth * scale * (instanceContext.hpBarLife / (float)npcContext.lifeMax)), (int)(4 * scale)), Color.DarkRed);
            if (npcContext.life > 0)
                Helpers.DrawRectangle(new Rectangle(drawCoords.X, drawCoords.Y, (int)(HPBarWidth * scale * (npcContext.life / (float)npcContext.lifeMax)), (int)(4 * scale)), color);
        }

        public static void DrawMixedThemeHB(NPC npc, Vector2 position, float scale)
        {
            var drawCoords = new Vector2((int)(position.X - Main.screenPosition.X), (int)(position.Y - Main.screenPosition.Y) +  4f);

            if (Main.LocalPlayer.gravDir == -1)
                drawCoords.Y = Main.screenHeight - drawCoords.Y;

            var color = (npcContext.boss || RORNPC.CountsAsBoss.Contains(npcContext.type)) ? Color.Red : Color.Yellow;
            var origin = MixedStyleBar.Value.Size() / 2f;
            int outSize = (int)Math.Max(2f * scale, 2f) * 2;

            int fillSpritePadding = 6;
            Main.spriteBatch.Draw(MixedStyleBar.Value, drawCoords, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            drawCoords.X += fillSpritePadding;
            if (instanceContext.hpBarLife > 0)
            {
                Main.spriteBatch.Draw(MixedStyleBarFill.Value, drawCoords, new Rectangle(fillSpritePadding, 0, (int)((MixedStyleBarFill.Value.Width - fillSpritePadding * 2)
                * (instanceContext.hpBarLife / (float)npcContext.lifeMax)), MixedStyleBarFill.Value.Height), Color.DarkRed * 2f, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(MixedStyleBarFill.Value, drawCoords, new Rectangle(fillSpritePadding, 0, (int)((MixedStyleBarFill.Value.Width - fillSpritePadding * 2)
                * (npcContext.life / (float)npcContext.lifeMax)), MixedStyleBarFill.Value.Height), color, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        private static void Main_DrawHealthBar(On.Terraria.Main.orig_DrawHealthBar orig, Main self, float X, float Y, int Health, int MaxHealth, float alpha, float scale, bool noFlip)
        {
            var state = ClientConfig.Instance.EnemyHBState;
            if (npcContext == null || instanceContext == null)
            {
                state = State.Vanilla;
            }

            switch (state)
            {
                default:
                    orig(self, X, Y, Health, MaxHealth, alpha, scale, noFlip);
                    break;

                case State.RORTheme:
                    DrawRORThemeHB(npcContext, new Vector2(X, Y), scale);
                    break;

                case State.MixedTheme:
                    DrawMixedThemeHB(npcContext, new Vector2(X, Y), scale);
                    break;
            }
        }

        public override void AI(NPC npc)
        {
            if (hpBarLife < npc.life)
            {
                hpBarLife = npc.life;
                hpBarLifeSetDelay = 0;
            }
            if (lastLife != npc.life)
            {
                lastLife = npc.life;
                hpBarLifeSetDelay = 30;
            }

            if (hpBarLifeSetDelay <= 1)
            {
                if (hpBarLife > npc.life)
                {
                    hpBarLifeSetDelay = -1;
                    hpBarLife -= (int)Math.Max(npc.lifeMax * 0.06f, 1f);
                    if (hpBarLife < npc.life)
                    {
                        hpBarLife = npc.life;
                        hpBarLifeSetDelay = 0;
                    }
                }
            }
            else
            {
                hpBarLifeSetDelay--;
            }
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            npcContext = npc;
            instanceContext = this;

            var old = position;
            var lowestY = npc.position.Y + (int)(npc.frame.Height * -0.25f) - 10;
            if (position.Y > lowestY)
                position.Y = lowestY;

            if (npcContext.IsElite())
                position.Y -= (int)(2f * scale);

            if (TopOffset.TryGetValue(npcContext.netID, out var offset))
            {
                position += offset;
            }

            if (ClientConfig.Instance.NPCDrawBuff)
            {
                if (ClientConfig.Instance.EnemyHBState == State.Vanilla)
                {
                    position += new Vector2(0f, 20f);
                }
                DrawDebuffs(npc, hbPosition, ref scale, ref position, ClientConfig.Instance.NPCDrawBuff_All);
            }

            return ClientConfig.Instance.EnemyHBState == State.Vanilla ? null : true;
        }

        public void DrawDebuffs(NPC npc, byte hbPosition, ref float scale, ref Vector2 position, bool renderAll)
        {
            var buffsToDraw = new List<Texture2D>();
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (npc.buffTime[i] <= 0 && npc.buffType[i] <= 0)
                {
                    continue;
                }

                if (!BuffIconData.TryGetValue(npc.buffType[i], out string texture))
                {
                    if (!renderAll)
                        continue;

                    if (npc.buffType[i] < Main.maxBuffTypes)
                    {
                        texture = RORMod.VanillaTexture + "Buff_" + npc.buffType[i];
                    }
                    else
                    {
                        texture = BuffLoader.GetBuff(npc.buffType[i]).Texture;
                    }
                }

                buffsToDraw.Add(ModContent.Request<Texture2D>(texture, AssetRequestMode.ImmediateLoad).Value);
            }

            float drawOffsetX = 0f;
            if (ClientConfig.Instance.EnemyHBState == State.Vanilla && buffsToDraw.Count % 2 == 0)
                drawOffsetX = 10f;

            for (int i = 0; i < buffsToDraw.Count; i++)
            {
                var t = buffsToDraw[i];
                float buffScale = 1f;
                int lw = t.Width > t.Height ? t.Width : t.Height;
                if (lw > 24)
                {
                    buffScale = 24f / lw;
                }
                buffScale *= scale;
                var drawCoords = position;
                drawCoords.X += drawOffsetX;
                if (ClientConfig.Instance.EnemyHBState == State.Vanilla)
                {
                    drawCoords += new Vector2(20f * scale * ((i + 1) / 2) * (i % 2 == 1 ? -1 : 1), -16f * scale);
                }
                else
                {
                    drawCoords += new Vector2(-10f * scale * NPC.maxBuffs + 10f * scale + 20f * scale * i, -16f * scale);
                }
                drawCoords -= Main.screenPosition;
                if (Main.LocalPlayer.gravDir == -1)
                    drawCoords.Y = Main.screenHeight - drawCoords.Y;
                Main.spriteBatch.Draw(t, drawCoords,
                    null, Color.White, 0f, t.Size() / 2f, buffScale, SpriteEffects.None, 0f);
            }
        }
    }
}
