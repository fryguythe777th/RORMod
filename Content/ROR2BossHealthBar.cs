using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace RiskOfTerrain.Content
{
    public class ROR2BossHealthBar : GlobalBossBar
    {
        public const char LEFT_WHITE_CORNER_BRACKET = '「';
        public const char RIGHT_WHITE_CORNER_BRACKET = '」';

        public enum State
        {
            Disabled,
            Enabled,
            Enabled_AlwaysUse,
        }

        public static Color hbColor;
        public static Color hbShieldColor;
        public static int hbWidth;
        public static int hbTop;
        public static int lastHP;
        public static int lastHPVisual;
        public static int lastHPTimer;
        public static int lastBoss;
        public static int life;
        public static int lifeMax;

        public static Dictionary<int, string> BossDesc { get; private set; }
        public static Dictionary<int, string> CustomNPCName { get; private set; }
        private static Dictionary<int, List<int>> SharedLifeGroups;

        public static void AddLifeGroup(List<int> group)
        {
            for (int i = 0; i < group.Count; i++)
            {
                SharedLifeGroups.Add(group[i], group);
            }
        }

        private string BossDescText(string key)
        {
            return "Mods.RiskOfTerrain.BossDesc." + key;
        }

        public override void SetStaticDefaults()
        {
            SharedLifeGroups = new Dictionary<int, List<int>>();
            AddLifeGroup(new List<int>() { NPCID.BrainofCthulhu, NPCID.Creeper, });
            AddLifeGroup(new List<int>() { NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, });
            AddLifeGroup(new List<int>() { NPCID.Retinazer, NPCID.Spazmatism, });
            AddLifeGroup(new List<int>() { NPCID.Golem, NPCID.GolemHead, });
            AddLifeGroup(new List<int>() { NPCID.MoonLordCore, NPCID.MoonLordHand, NPCID.MoonLordHead, });
            AddLifeGroup(new List<int>() { NPCID.MartianSaucerCore, NPCID.MartianSaucerCannon, NPCID.MartianSaucerTurret, });

            BossDesc = new Dictionary<int, string>()
            {
                [NPCID.EyeofCthulhu] = BossDescText("EyeofCthulhu"),
                [NPCID.EaterofWorldsHead] = BossDescText("EaterofWorlds"),
                [NPCID.EaterofWorldsBody] = BossDescText("EaterofWorlds"),
                [NPCID.EaterofWorldsTail] = BossDescText("EaterofWorlds"),
                [NPCID.BrainofCthulhu] = BossDescText("BrainofCthulhu"),
                [NPCID.KingSlime] = BossDescText("KingSlime"),
                [NPCID.SkeletronHead] = BossDescText("Skeletron"),
                [NPCID.QueenBee] = BossDescText("QueenBee"),
                [NPCID.WallofFlesh] = BossDescText("WallOfFlesh"),
                // Solus Probes
                [NPCID.Retinazer] = BossDescText("MechanicalBoss"),
                [NPCID.Spazmatism] = BossDescText("MechanicalBoss"),
                [NPCID.SkeletronPrime] = BossDescText("MechanicalBoss"),
                [NPCID.TheDestroyer] = BossDescText("MechanicalBoss"),
                [NPCID.Plantera] = BossDescText("Plantera"),
                [NPCID.Golem] = BossDescText("Golem"),
                [NPCID.GolemHead] = BossDescText("Golem"),
                [NPCID.CultistBoss] = BossDescText("LunaticCultist"),
                [NPCID.MoonLordCore] = BossDescText("MoonLord"),
                [NPCID.MoonLordHand] = BossDescText("MoonLord"),
                [NPCID.MoonLordHead] = BossDescText("MoonLord"),
                [NPCID.HallowBoss] = BossDescText("EmpressOfLight"),
                [NPCID.QueenSlimeBoss] = BossDescText("QueenSlime"),
                [NPCID.Deerclops] = BossDescText("Deerclops"),
                [NPCID.DD2Betsy] = BossDescText("CollabEvent"),
                [NPCID.DD2OgreT2] = BossDescText("CollabEvent"),
                [NPCID.DD2OgreT3] = BossDescText("CollabEvent"),
                [NPCID.DD2DarkMageT1] = BossDescText("CollabEvent"),
                [NPCID.DD2DarkMageT3] = BossDescText("CollabEvent"),
                [NPCID.TorchGod] = "Hi :D",
                [NPCID.DukeFishron] = BossDescText("DukeFishron"),
                [NPCID.MartianSaucer] = BossDescText("MartianSaucer"),
                [NPCID.MartianSaucerCore] = BossDescText("MartianSaucer"),
                [NPCID.LunarTowerSolar] = BossDescText("Pillars"),
                [NPCID.LunarTowerVortex] = BossDescText("Pillars"),
                [NPCID.LunarTowerNebula] = BossDescText("Pillars"),
                [NPCID.LunarTowerStardust] = BossDescText("Pillars"),
            };

            CustomNPCName = new Dictionary<int, string>()
            {
                [NPCID.EaterofWorldsHead] = "NPCName.EaterofWorldsHead",
                [NPCID.EaterofWorldsBody] = "NPCName.EaterofWorldsHead",
                [NPCID.EaterofWorldsTail] = "NPCName.EaterofWorldsHead",
                [NPCID.DD2Betsy] = "NPCName.DD2Betsy",
                [NPCID.DD2OgreT2] = "NPCName.DD2OgreT2",
                [NPCID.DD2OgreT3] = "NPCName.DD2OgreT3",
                [NPCID.DD2DarkMageT1] = "NPCName.DD2DarkMageT1",
                [NPCID.DD2DarkMageT3] = "NPCName.DD2DarkMageT3",
                [NPCID.TorchGod] = "NPCName.TorchGod",
                [NPCID.Retinazer] = "Enemies.TheTwins",
                [NPCID.Spazmatism] = "Enemies.TheTwins",
                [NPCID.GolemHead] = "NPCName.Golem",
                [NPCID.Golem] = "NPCName.Golem",
                [NPCID.MoonLordCore] = "Enemies.MoonLord",
                [NPCID.MoonLordHand] = "Enemies.MoonLord",
                [NPCID.MoonLordHead] = "Enemies.MoonLord",
            };

            hbColor = new Color(128, 1, 1, 255);
            hbShieldColor = new Color(1, 70, 128, 255);
            hbWidth = 480 * 2;
            hbTop = 24;
        }

        private void UpdateLife(NPC npc)
        {
            if (SharedLifeGroups.TryGetValue(npc.netID, out var list))
            {
                bool updateMax = lifeMax <= npc.lifeMax;
                if (npc.type == NPCID.MoonLordCore || npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead)
                {
                    updateMax = true;
                }
                if (updateMax)
                {
                    lifeMax = 0;
                }
                life = 0;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && list.Contains(Main.npc[i].netID))
                    {
                        if (updateMax)
                            lifeMax += Main.npc[i].lifeMax;
                        if ((Main.npc[i].type == NPCID.MoonLordHand || Main.npc[i].type == NPCID.MoonLordHead)
                            && (int)Main.npc[i].ai[0] == -2)
                        {
                            continue;
                        }
                        life += Main.npc[i].life;
                    }
                }
            }
            else
            {
                lifeMax = npc.lifeMax;
                life = npc.life;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams)
        {
            var state = ClientConfig.Instance.BossHealthbarActive;
            if (state == State.Enabled_AlwaysUse || (state == State.Enabled && ((npc.boss && npc.BossBar == null) || BossDesc.ContainsKey(npc.netID))))
            {
                if (lastBoss != npc.type)
                {
                    if (SharedLifeGroups.TryGetValue(npc.netID, out var list))
                    {
                        if (list.Contains(lastBoss))
                        {
                            goto PostReset;
                        }
                    }
                    lastBoss = npc.netID;
                    lastHPVisual = life;
                    lifeMax = 0;
                }

            PostReset:
                UpdateLife(npc);

                if (life != lastHP)
                {
                    lastHP = life;
                    if (lastHPTimer <= 0)
                        lastHPVisual = life;
                    lastHPTimer = 32;
                }
                else
                {
                    if (lastHPTimer <= 0)
                    {
                        lastHP = life;
                        lastHPVisual -= lifeMax / 50;
                        lastHPTimer = 0;
                    }
                    else
                    {
                        lastHPTimer--;
                    }
                }

                lastHPVisual = Math.Min(Math.Max(life, lastHPVisual), lifeMax);

                float middleX = Main.screenWidth / 2f;
                float topY = 24f;

                var pixel = TextureAssets.MagicPixel.Value;

                var bottom = ClientConfig.Instance.BossHealthbarBottom;
                if (bottom)
                {
                    topY += 24;
                }
                var drawPosition = new Vector2(middleX - hbWidth / 2f, bottom ? Main.screenHeight - topY : topY);
                var frame = new Rectangle(0, 0, 1, 1);
                var scale = new Vector2(hbWidth, 8f);

                Main.spriteBatch.Draw(pixel, drawPosition - new Vector2(0f, 4f), frame, new Color(1, 1, 1, 255), 0f, Vector2.Zero, scale + new Vector2(0f, 8f), SpriteEffects.None, 0f);
                for (int i = 0; i < 3; i++)
                {
                    Main.spriteBatch.Draw(pixel, drawPosition - new Vector2(0f, i * 2f), frame, hbColor * 0.4f * (1 - i * 0.25f), 0f, Vector2.Zero, new Vector2(scale.X * (lastHPVisual / (float)lifeMax), scale.Y) + new Vector2(0f, i * 4f), SpriteEffects.None, 0f);
                }
                if ((drawParams.Life / drawParams.LifeMax) > 0f)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Main.spriteBatch.Draw(pixel, drawPosition - new Vector2(0f, i * 2f), frame, hbColor * (1 - i * 0.25f), 0f, Vector2.Zero, new Vector2(scale.X * (drawParams.Life / drawParams.LifeMax), scale.Y + i * 4f), SpriteEffects.None, 0f);
                    }
                }
                if ((drawParams.Shield / drawParams.ShieldMax)/* tModPorter Note: Removed. Suggest: Shield / ShieldMax */ > 0f)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Main.spriteBatch.Draw(pixel, drawPosition - new Vector2(0f, i * 2f), frame, hbShieldColor * (1 - i * 0.25f), 0f, Vector2.Zero, new Vector2(scale.X * (drawParams.Shield / drawParams.ShieldMax), scale.Y + i * 4f), SpriteEffects.None, 0f);
                    }
                }

                DrawText(string.Format("{0}{2}{1}", Math.Max(life, 0), lifeMax, '/'), new Vector2(middleX, drawPosition.Y + 8), Vector2.One);

                string text = Lang.GetNPCName(npc.netID).Value;
                if (npc.HasGivenName)
                {
                    text = npc.GivenName;
                }
                else if (CustomNPCName.TryGetValue(npc.netID, out string text2))
                {
                    text = Language.GetTextValue(text2);
                }

                if (bottom)
                {
                    var v = Vector2.Zero;
                    if (BossDesc.TryGetValue(npc.netID, out string key))
                    {
                        v = DrawText(Language.GetTextValue(key), new Vector2(middleX, drawPosition.Y - 20), Vector2.One * 0.75f);
                    }
                    text = string.Format("{1}{0}{2}", text, LEFT_WHITE_CORNER_BRACKET, RIGHT_WHITE_CORNER_BRACKET);

                    DrawText(text, new Vector2(middleX, drawPosition.Y - (20 + v.Y)), Vector2.One * 0.95f);
                }
                else
                {
                    text = string.Format("{1}{0}{2}", text, LEFT_WHITE_CORNER_BRACKET, RIGHT_WHITE_CORNER_BRACKET);

                    var textMeasurement = DrawText(text, new Vector2(middleX, drawPosition.Y + 30), Vector2.One * 0.95f);

                    if (BossDesc.TryGetValue(npc.netID, out string key))
                    {
                        DrawText(Language.GetTextValue(key), new Vector2(middleX, drawPosition.Y + (20 + textMeasurement.Y)), Vector2.One * 0.75f);
                    }
                }
                return false;
            }
            return true;
        }

        private static Vector2 DrawText(string text, Vector2 pos, Vector2 scale)
        {
            var font = FontAssets.MouseText.Value;
            var textMeasurement = font.MeasureString(text);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, pos + new Vector2(0f, 2f * scale.Y), Color.Black, 0f, textMeasurement / 2f, scale);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, pos, Color.White, 0f, textMeasurement / 2f, scale);
            return textMeasurement;
        }
    }
}