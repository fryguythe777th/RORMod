using Microsoft.Xna.Framework;
using RORMod.Common.Networking;
using RORMod.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod
{
	public class RORMod : Mod
	{
        public static bool chaos;
        public static bool command;
        public static bool death;
        public static bool dissonance;
        public static bool enigma;
        public static bool evolution;
        public static bool frailty;
        public static bool glass;
        public static bool honor;
        public static bool kin;
        public static bool metamorphosis;
        public static bool sacrifice;
        public static bool soul;
        public static bool spite;
        public static bool swarms;
        public static bool vengeance;

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
                    ROR2HealthBar.AddLifeGroup(args[1] as List<int>);
                    return "Success";
                case "CustomName":
                    ROR2HealthBar.CustomNPCName.Add((int)args[1], args[2] as string);
                    return "Success";
                case "BossDesc":
                    ROR2HealthBar.BossDesc.Add((int)args[1], args[2] as string);
                    return "Success";
            }
            return "Failiure";
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