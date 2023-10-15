using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Commando
{
    public class CommandoGrenadeWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Yellow;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 30;
            Item.knockBack = 10;

            Item.shootSpeed = 12;
            Item.shoot = ModContent.ProjectileType<GrenadeProj>();
        }
    }
}