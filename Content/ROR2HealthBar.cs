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
    public class ROR2HealthBar : GlobalNPC
    {
        public enum State
        {
            Disabled,
            Enabled,
            BuffsOnly,
            HealthbarOnly,
        }

        public static Dictionary<int, string> BuffIcons { get; private set; }

        public const int HPBarWidth = 60;

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;
        public int lastLife;
        public int hpBarLife;
        public int hpBarLifeSetDelay;
        private static NPC npcContext;
        private static ROR2HealthBar instanceContext;

        public override void Load()
        {
            string t = RORMod.AssetsPath + "UI/HealthBar/Buff_";
            BuffIcons = new Dictionary<int, string>()
            {
                [BuffID.Poisoned] = $"{t}{BuffID.Poisoned}",
                [BuffID.OnFire] = $"{t}{BuffID.OnFire}",
                [BuffID.OnFire3] = $"{t}{BuffID.OnFire}",
                [BuffID.Confused] = $"{t}{BuffID.Confused}",
                [BuffID.CursedInferno] = $"{t}{BuffID.CursedInferno}",
                [BuffID.Frostburn] = $"{t}{BuffID.Frostburn}",
                [BuffID.Frostburn2] = $"{t}{BuffID.Frostburn}",
                [BuffID.Ichor] = $"{t}{BuffID.Ichor}",
                [BuffID.Venom] = $"{t}{BuffID.Venom}",
                [BuffID.Midas] = $"{t}{BuffID.Midas}",
                [BuffID.Wet] = $"{t}{BuffID.Wet}",
                [BuffID.Stinky] = $"{t}{BuffID.Stinky}",
                [BuffID.Slimed] = $"{t}{BuffID.Slimed}",
                [BuffID.ShadowFlame] = $"{t}{BuffID.ShadowFlame}",
                [BuffID.Oiled] = $"{t}{BuffID.Oiled}",
            };
            On.Terraria.Main.DrawHealthBar += Main_DrawHealthBar;
        }

        private static void Main_DrawHealthBar(On.Terraria.Main.orig_DrawHealthBar orig, Main self, float X, float Y, int Health, int MaxHealth, float alpha, float scale, bool noFlip)
        {
            if (ClientConfig.Instance.RORHealthbar == State.Disabled || ClientConfig.Instance.RORHealthbar == State.BuffsOnly || npcContext == null || instanceContext == null)
            {
                orig(self, X, Y, Health, MaxHealth, alpha, scale, noFlip);
                return;
            }

            if (ClientConfig.Instance.RORHealthbar == State.BuffsOnly)
            {
                return;
            }

            var position = new Vector2(X, Y);
            var drawCoords = new Point((int)(position.X - Main.screenPosition.X) - (int)(HPBarWidth / 2 * scale), (int)(position.Y - Main.screenPosition.Y));

            var color = (npcContext.boss || RORNPC.CountsAsBoss.Contains(npcContext.type)) ? Color.Red : Color.Yellow;
            int outSize = (int)Math.Max(2f * scale, 2f) * 2;

            if (npcContext.IsElite())
            {
                Helpers.DrawRectangle(new Rectangle(drawCoords.X - outSize, drawCoords.Y - outSize, (int)(HPBarWidth * scale) + outSize * 2, (int)(4 * scale) + outSize * 2), Color.White);
            }
            Helpers.DrawRectangle(new Rectangle(drawCoords.X - outSize / 2, drawCoords.Y - outSize / 2, (int)(HPBarWidth * scale) + outSize, (int)(4 * scale) + outSize), Color.Black);
            if (instanceContext.hpBarLife > 0)
                Helpers.DrawRectangle(new Rectangle(drawCoords.X, drawCoords.Y, (int)(HPBarWidth * scale * (instanceContext.hpBarLife / (float)npcContext.lifeMax)), (int)(4 * scale)), Color.DarkRed);
            if (npcContext.life > 0)
                Helpers.DrawRectangle(new Rectangle(drawCoords.X, drawCoords.Y, (int)(HPBarWidth * scale * (npcContext.life / (float)npcContext.lifeMax)), (int)(4 * scale)), color);
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

            switch (npc.type) 
            {
                case NPCID.TheDestroyer:
                case NPCID.TheDestroyerBody:
                case NPCID.TheDestroyerTail:
                    return null;
            }

            if (ClientConfig.Instance.RORHealthbar == State.Disabled)
                return null;

            switch (npc.type)
            {
                case NPCID.WallofFleshEye:
                    return false;
            }

            var lowestY = npc.position.Y + (int)(npc.frame.Height * -0.25f) - 10;
            if (position.Y > lowestY)
                position.Y = lowestY;

            if (npcContext.IsElite())
                position.Y -= (int)(2f * scale);

            switch (npcContext.type)
            {
                case NPCID.MoonLordHead:
                    {
                        position.Y += 70;
                    }
                    break;

                case NPCID.MoonLordHand:
                    {
                        position.Y += 40;
                    }
                    break;
            }

            if ((ClientConfig.Instance.RORHealthbar != State.Enabled && ClientConfig.Instance.RORHealthbar != State.BuffsOnly) || npc.life >= npc.lifeMax)
            {
                return true;
            }

            DrawDebuffs(npc, hbPosition, ref scale, ref position, !ClientConfig.Instance.RORHealthbarDrawBuff);

            return null;
        }

        public void DrawDebuffs(NPC npc, byte hbPosition, ref float scale, ref Vector2 position, bool renderAll)
        {
            int buffsDrawn = 0;
            int width = HPBarWidth + 20;
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (npc.buffTime[i] <= 0 && npc.buffType[i] <= 0)
                {
                    continue;
                }

                if (!BuffIcons.TryGetValue(npc.buffType[i], out string texture))
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

                DrawDebuff(ModContent.Request<Texture2D>(texture, AssetRequestMode.ImmediateLoad).Value, scale, position, width, i);

                buffsDrawn++;
            }
        }
        public void DrawDebuff(Texture2D t, float scale, Vector2 position, int width, int i)
        {
            float buffScale = 1f;
            int lw = t.Width > t.Height ? t.Width : t.Height;
            if (lw > 16)
            {
                buffScale = 12f / lw;
            }
            buffScale *= scale;
            Main.spriteBatch.Draw(t, position + new Vector2(-width / 2f * scale + width / NPC.maxBuffs * i * scale, -16f * scale) - Main.screenPosition,
                null, Color.White, 0f, t.Size() / 2f, buffScale, SpriteEffects.None, 0f);
        }
    }
}
