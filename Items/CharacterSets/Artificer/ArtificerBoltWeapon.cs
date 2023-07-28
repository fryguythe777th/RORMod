using System;
using Microsoft.Xna.Framework;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Artificer
{
    public class ArtificerBoltWeapon: ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Yellow;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BeeArrow;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 20;
            Item.shootSpeed = 10f;
            Item.mana = 5;
        }

        public int fireState = 1; //1 = fireball, 2 = plasmaball, 3 = flamethrower, 4 = nanobomb
        public float charge = 0;
        public const int ChargeMaximum = 100;
        public int specialAttackTimer = 30;
        public int rightClickSwitchTimer = 90;

        public override void HoldItem(Player player)
        {
            if (fireState < 3)
            {
                charge += 1 * (Math.Abs(player.velocity.X / 60) + Math.Abs(player.velocity.Y / 60));
            }
            if (charge > ChargeMaximum)
            {
                charge = ChargeMaximum;
            }

            if (fireState == 1 || fireState == 2)
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
            }
            else
            {
                Item.useTime = 4;
                Item.useAnimation = 4;
            }

            if (Main.mouseRight)
            {
                if (rightClickSwitchTimer == 90) 
                {
                    if (fireState == 1)
                    {
                        if (charge >= ChargeMaximum)
                        {
                            fireState = 3;
                            charge = 0;
                        }
                        else
                        {
                            fireState = 2;
                        }
                    }
                    else if (fireState == 2)
                    {
                        if (charge >= ChargeMaximum)
                        {
                            fireState = 4;
                            charge = 0;
                        }
                        else
                        {
                            fireState = 1;
                        }
                    }
                    rightClickSwitchTimer = 0;
                }
            }

            if (rightClickSwitchTimer < 90)
            {
                rightClickSwitchTimer++;
            }
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (fireState == 3)
            {
                mult = 0.1f;
            }
            if (fireState == 4 && specialAttackTimer > 1)
            {
                mult = 0f;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (fireState == 1)
            {
                type = ModContent.ProjectileType<FireBolt>();
                
                SoundStyle fireball = new SoundStyle();
                fireball.SoundPath = SoundID.Item45.SoundPath;
                fireball.PitchVariance = 1f;
                fireball.Pitch = 0;
                fireball.Volume = 0.5f;
                SoundEngine.PlaySound(fireball);
            }
            else if (fireState == 2)
            {
                type = ModContent.ProjectileType<PlasmaBolt>();
                
                SoundStyle plasmaball = new SoundStyle();
                plasmaball.SoundPath = SoundID.Item43.SoundPath;
                plasmaball.PitchVariance = 1f;
                plasmaball.Pitch = 0;
                plasmaball.Volume = 0.5f;
                SoundEngine.PlaySound(plasmaball);
            }
            else if (fireState == 3)
            {
                type = ProjectileID.Flames;
                specialAttackTimer--;
                if (specialAttackTimer == 0)
                {
                    specialAttackTimer = 30;
                    fireState = 1;
                }
            }
            else if (fireState == 4)
            {
                specialAttackTimer--;
                if (specialAttackTimer == 0)
                {
                    specialAttackTimer = 30;
                    fireState = 2;
                    type = ProjectileID.StarWrath;
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (fireState == 4 && specialAttackTimer > 0)
            {
                return false;
            }
            return true;
        }
    }
}