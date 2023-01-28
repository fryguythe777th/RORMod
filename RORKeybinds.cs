using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class RORKeybinds : ModSystem
{

    //public static ModKeybind SprintKey { get; private set; }
    public static ModKeybind AmmoSwapKey { get; private set; }

    public override void Load()
    {
        //SprintKey = KeybindLoader.RegisterKeybind(Mod, "Sprint Keybind", "X");
        AmmoSwapKey = KeybindLoader.RegisterKeybind(Mod, "Backup Magazine Swap", "MouseRight");
    }

    public override void Unload()
    {
        //SprintKey = null;
        AmmoSwapKey = null;
    }
}