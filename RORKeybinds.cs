using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class RORKeybinds : ModSystem
{

    public static ModKeybind HeadsetKey { get; private set; }
    public static ModKeybind AmmoSwapKey { get; private set; }
    public static ModKeybind ArmorEffectKey { get; private set; }
    public static ModKeybind AfterburnerDashKey { get; private set; }

    public override void Load()
    {
        HeadsetKey = KeybindLoader.RegisterKeybind(Mod, "H3AD-5T Keybind", "X");
        AmmoSwapKey = KeybindLoader.RegisterKeybind(Mod, "Backup Magazine Swap", "Mouse2");
        ArmorEffectKey = KeybindLoader.RegisterKeybind(Mod, "Armor Effect Key", "Z");
        AfterburnerDashKey = KeybindLoader.RegisterKeybind(Mod, "Hardlight Afterburner Dash Key", "Mouse3");
    }

    public override void Unload()
    {
        HeadsetKey = null;
        AmmoSwapKey = null;
        ArmorEffectKey = null;
        AfterburnerDashKey = null;
    }
}