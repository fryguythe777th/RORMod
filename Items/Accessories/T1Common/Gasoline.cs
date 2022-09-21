using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories.T1Common
{
    public class Gasoline : ModItem
    {
        public static HashSet<int> FireDebuffsForGasolineDamageOverTime { get; private set; }

        public override void Load()
        {
            FireDebuffsForGasolineDamageOverTime = new HashSet<int>()
            {
                BuffID.OnFire,
                BuffID.OnFire3,
            };
        }

        public override void Unload()
        {
            FireDebuffsForGasolineDamageOverTime?.Clear();
            FireDebuffsForGasolineDamageOverTime = null;
        }

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ROR().accGasoline = Item;
        }
    }
}