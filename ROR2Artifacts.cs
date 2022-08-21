using Microsoft.Xna.Framework;
using ROR2Artifacts.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROR2Artifacts
{
	public class ROR2Artifacts : Mod
	{
		public static bool ChaosActive;
		public static bool CommandActive;
		public static bool DeathActive;
		public static bool DissonanceActive;
		public static bool EnigmaActive;
		public static bool EvolutionActive;
		public static bool FrailtyActive;
		public static bool GlassActive;
		public static bool HonorActive;
		public static bool KinActive;
		public static bool MetamorphosisActive;
		public static bool SacrificeActive;
		public static bool SoulActive;
		public static bool SpiteActive;
		public static bool SwarmsActive;
		public static bool VenganceActive;

        public static Color BossSummonMessage => new Color(175, 75, 255, 255);

        public override object Call(params object[] args)
        {
            switch (args[0] as string)
            {
                case "HPPool":
                    HealthBar.AddLifeGroup(args[1] as List<int>);
                    return "Success";
                case "CustomName":
                    HealthBar.CustomNPCName.Add((int)args[1], args[2] as string);
                    return "Success";
                case "BossDesc":
                    HealthBar.BossDesc.Add((int)args[1], args[2] as string);
                    return "Success";
            }
            return "Failiure";
        }

        public static void Broadcast(string text, Color color)
        {
            text = "Mods.ROR2Artifacts." + text;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(text), color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), color);
            }
        }
        public static void Broadcast(string text, Color color, params object[] args)
        {
            text = "Mods.ROR2Artifacts." + text;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(text, args), color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text, args), color);
            }
        }
        public static void BroadcastKeys(string text, Color color, params object[] args)
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
            Broadcast(text, color, args);
        }
    }
}