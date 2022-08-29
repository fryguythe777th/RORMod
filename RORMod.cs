using Microsoft.Xna.Framework;
using RORMod.Content;
using RORMod.NPCs;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod
{
    public class RORMod : Mod
    {
        public const string VanillaTexture = "Terraria/Images/";
        public const string BlankTexture = "RORMod/Assets/None";
        public const string AssetsPath = "RORMod/Assets/";
        public const string SoundsPath = AssetsPath + "Sounds/";

        public static RORMod Instance { get; private set; }

        public static Color BossSummonMessage => new Color(175, 75, 255, 255);

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
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
                        Main.player[reader.ReadInt32()].ROR().TougherTimesDodge();
                    }
                    break;

                case PacketType.SyncRORPlayer:
                    {
                        Main.player[reader.ReadInt32()].ROR().RecieveChanges(reader);
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

        public static ModPacket GetPacket(PacketType type)
        {
            var p = Instance.GetPacket();
            p.Write((byte)type);
            return p;
        }

        public static void BroadcastMessage(string text, Color color)
        {
            text = "Mods.RORMod." + text;
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
            text = "Mods.RORMod." + text;
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
    }
}