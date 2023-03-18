using Microsoft.Xna.Framework;
using RiskOfTerrain.Content;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain
{
    public class RiskOfTerrain : Mod
    {
        public const string VanillaTexture = "Terraria/Images/";
        public const string BlankTexture = AssetsPath + "None";
        public const string AssetsPath = "RiskOfTerrain/Assets/";
        public const string SoundsPath = AssetsPath + "Sounds/";

        public static RiskOfTerrain Instance { get; private set; }
        public static float globalProcRate;

        public static List<(Func<bool>, float)> PriceModifiers { get; private set; }

        public static Color BossSummonMessage => new Color(175, 75, 255, 255);

        public static int numCelestinesIngame = 0;

        public override void Load()
        {
            Instance = this;
            PriceModifiers = new List<(Func<bool>, float)>()
            {
                (() => NPC.downedBoss1, 0.25f),
                (() => NPC.downedBoss2, 0.5f),
                (() => NPC.downedBoss3, 0.5f),
                (() => NPC.downedQueenBee, 0.1f),
                (() => NPC.downedSlimeKing, 0.1f),
                (() => NPC.downedDeerclops, 0.1f),
                (() => NPC.downedMechBossAny, 2f),
                (() => NPC.downedMechBoss1, 0.5f),
                (() => NPC.downedMechBoss2, 0.5f),
                (() => NPC.downedMechBoss3, 0.5f),
                (() => NPC.downedQueenSlime, 0.1f),
                (() => NPC.downedPlantBoss, 4f),
                (() => NPC.downedGolemBoss, 0.5f),
                (() => NPC.downedFishron, 0.25f),
                (() => NPC.downedEmpressOfLight, 0.25f),
                (() => NPC.downedAncientCultist, 0.25f),
                (() => NPC.downedGoblins, 0.1f),
                (() => NPC.downedPirates, 0.1f),
                (() => NPC.downedMartians, 0.25f),
            };
        }

        public override void Unload()
        {
            PriceModifiers?.Clear();
            PriceModifiers = null;
            Instance = null;
        }

        public override object Call(params object[] args)
        {
            switch (args[0] as string)
            {
                case "HPPool":
                    ROR2BossHealthBar.AddLifeGroup(args[1] as List<int>);
                    return "Success";
                case "CustomName":
                    ROR2BossHealthBar.CustomNPCName.Add((int)args[1], args[2] as string);
                    return "Success";
                case "BossDesc":
                    ROR2BossHealthBar.BossDesc.Add((int)args[1], args[2] as string);
                    return "Success";
            }
            return "Failiure";
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var t = (PacketType)reader.ReadByte();
            switch (t)
            {
                case PacketType.SyncRORNPC:
                    {
                        int npc = reader.ReadInt32();
                        Main.npc[npc].GetGlobalNPC<RORNPC>().Receive(npc, reader);
                    }
                    break;

                case PacketType.TougherTimesDodge:
                    {
                        TougherTimes.DoDodgeEffect(Main.player[reader.ReadInt32()]);
                    }
                    break;

                case PacketType.SyncRORPlayer:
                    {
                        Main.player[reader.ReadInt32()].ROR().RecieveChanges(reader);
                    }
                    break;

                case PacketType.OnKillEffect:
                    {
                        var player = Main.player[reader.ReadInt32()];
                        int type = reader.ReadInt32();
                        var position = reader.ReadVector2();
                        int w = reader.ReadInt32();
                        int h = reader.ReadInt32();
                        int lifeMax = reader.ReadInt32();
                        int lastHitDamage = reader.ReadInt32();
                        byte misc = reader.ReadByte();
                        float value = reader.ReadSingle();
                        bool friendly = reader.ReadBoolean();
                        bool spawnedfromstatue = reader.ReadBoolean();

                        player.ROR().Accessories.OnKillEnemy(player, new OnKillInfo()
                        {
                            type = type,
                            position = position,
                            width = w,
                            height = h,
                            lifeMax = lifeMax,
                            lastHitDamage = lastHitDamage,
                            miscInfo = misc,
                            value = value,
                            friendly = friendly,
                            spawnedFromStatue = spawnedfromstatue
                        });
                    }
                    break;
            }
        }

        internal static SoundStyle GetSounds(string name, int num, float volume = 1f, float pitch = 0f, float variance = 0f)
        {
            return new SoundStyle(SoundsPath + name, 0, num) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
        }
        internal static SoundStyle GetSound(string name, float volume = 1f, float pitch = 0f, float variance = 0f)
        {
            return new SoundStyle(SoundsPath + name) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
        }

        internal static ModKeybind RegisterKeybind(string name, string defaultKey)
        {
            var key = KeybindLoader.RegisterKeybind(ModContent.GetInstance<RiskOfTerrain>(), name, defaultKey);
            return key;
        }

        public static ModPacket GetPacket(PacketType type)
        {
            var p = Instance.GetPacket();
            p.Write((byte)type);
            return p;
        }

        public static void BroadcastMessage(string text, Color color)
        {
            text = "Mods.RiskOfTerrain." + text;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(text), color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), color);
            }
        }
        public static void BroadcastMessage(string text, Color color, params object[] args)
        {
            text = "Mods.RiskOfTerrain." + text;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(text, args), color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text, args), color);
            }
        }
        public static void BroadcastMessageKeys(string text, Color color, params object[] args)
        {
            if (args != null)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] is string)
                        {
                            args[i] = Language.GetTextValue(args[i] as string);
                        }
                        if (args[i] is NetworkText)
                        {
                            args[i] = Language.GetTextValue((args[i] as NetworkText).ToString());
                        }
                    }
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] is string)
                        {
                            args[i] = NetworkText.FromKey(args[i] as string);
                        }
                    }
                }
            }
            BroadcastMessage(text, color, args);
        }

        public static int CalculateChestPrice(float chestMult)
        {
            int basePrice = Item.buyPrice(gold: 2, silver: 50);
            if (Main.expertMode)
            {
                basePrice *= 2;
            }
            if (Main.hardMode)
            {
                basePrice *= 2;
            }
            int finalPrice = basePrice;
            foreach (var price in PriceModifiers)
            {
                if (price.Item1())
                {
                    finalPrice += (int)(basePrice * price.Item2);
                }
            }
            finalPrice = (int)(chestMult * finalPrice);
            return finalPrice;
        }
    }
}