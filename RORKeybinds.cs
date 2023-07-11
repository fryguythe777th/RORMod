using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class RORKeybinds : ModSystem
{

    public static ModKeybind HeadsetKey { get; private set; }
    public static ModKeybind AmmoSwapKey { get; private set; }

    public override void Load()
    {
        HeadsetKey = KeybindLoader.RegisterKeybind(Mod, "H3AD-5T Keybind", "X");
        AmmoSwapKey = KeybindLoader.RegisterKeybind(Mod, "Backup Magazine Swap", "Mouse2");
    }

    public override void Unload()
    {
        HeadsetKey = null;
        AmmoSwapKey = null;
    }
}