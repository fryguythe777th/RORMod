using Microsoft.Xna.Framework;
using RORMod.NPCs;
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
            Item.value = Item.buyPrice(gold: 1);
        }

        public override bool? UseItem(Player player)
        {
            return null;

            for (int i = 0; i < RORNPC.RegisteredElites.Count; i++)
            {
                int x = i % 4;
                int y = i / 4;

                var n =NPC.NewNPCDirect(player.GetSource_ItemUse(Item), player.Center + new Vector2(80f * x, 80f * y), NPCID.EaterofSouls);
                n.aiStyle = -1;
                n.rotation = 0f;
                n.velocity = Vector2.Zero;
                n.damage = 0;
                n.dontTakeDamage = true;
                n.friendly = true;
                n.GetGlobalNPC(RORNPC.RegisteredElites[i]).Active = true;
            }
        }
    }
}