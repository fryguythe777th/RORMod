using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs.Debuff
{
    public class ShatteredDebuff : ModBuff
    {
        //the only purpose of this is to make death mark work with it
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}