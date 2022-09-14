using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Consumable
{
    public class BisonSteak : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 5;
            ItemID.Sets.IsFood[Type] = true;
            ItemID.Sets.FoodParticleColors[Type] = new Color[] { new Color(180, 60, 60, 255), new Color(200, 90, 83, 255), new Color(205, 140, 125, 255), };
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
        }

        public override void SetDefaults()
        {
            Item.DefaultToFood(20, 20, BuffID.WellFed3, 36000);
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 50);
        }
    }
}