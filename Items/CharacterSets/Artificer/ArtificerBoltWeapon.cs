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

        public int fireState = 1; //1 = fireball, 2 = plasmaball, 3 = ice spear, 4 = flamethrower, 5 = nanobomb, 6 = ice wall
        public float charge = 0;
        public const int ChargeMaximum = 100;
        public int specialAttackTimer = 30;
        public int rightClickSwitchTimer = 90;

        private void SpawnElementIcon(Player player, int switchingTo)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ElementIcon>(), 0, 0, player.whoAmI, 1, switchingTo);
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ElementIcon>(), 0, 0, player.whoAmI, 2, switchingTo);
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ElementIcon>(), 0, 0, player.whoAmI, 3, switchingTo);
        }

        public override void HoldItem(Player player)
        {
            if (fireState < 4)
            {
                charge += 1 * (Math.Abs(player.velocity.X / 60) + Math.Abs(player.velocity.Y / 60));
            }
            if (charge > ChargeMaximum)
            {
                charge = ChargeMaximum;
            }

            player.ROR().artificerCharge = charge;

            if (fireState == 1 || fireState == 2)
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else if (fireState == 3)
            {
                Item.useTime = 40;
                Item.useAnimation = 20;
                Item.useStyle = ItemUseStyleID.Swing;
            }
            else if (fireState == 4 || fireState == 5)
            {
                Item.useTime = 4;
                Item.useAnimation = 4;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else if (fireState == 6)
            {
                Item.useTime = 120;
                Item.useAnimation = 120;
                Item.useStyle = ItemUseStyleID.Shoot;
            }

            if (Main.mouseRight)
            {
                if (rightClickSwitchTimer == 90) 
                {
                    if (fireState == 1)
                    {
                        if (charge >= ChargeMaximum)
                        {
                            fireState = 4;
                            charge = 0;
                        }
                        else
                        {
                            SpawnElementIcon(player, 2);
                            fireState = 2;
                        }
                    }
                    else if (fireState == 2)
                    {
                        if (charge >= ChargeMaximum)
                        {
                            fireState = 5;
                            charge = 0;
                        }
                        else
                        {
                            SpawnElementIcon(player, 3);
                            fireState = 3;
                        }
                    }
                    else if (fireState == 3)
                    {
                        if (charge >= ChargeMaximum)
                        {
                            fireState = 6;
                            charge = 0;
                        }
                        else
                        {
                            SpawnElementIcon(player, 1);
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
                mult = 2f;
            }
            if (fireState == 4)
            {
                mult = 0.1f;
            }
            if (fireState == 5 && specialAttackTimer > 1)
            {
                mult = 0f;
            }
            if (fireState == 6)
            {
                mult = 5f;
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
                type = ModContent.ProjectileType<IceSpear>();
                velocity *= 1.5f;

                SoundStyle icespear = new SoundStyle();
                icespear.SoundPath = SoundID.Item28.SoundPath;
                icespear.PitchVariance = 1f;
                icespear.Pitch = 0;
                icespear.Volume = 2f;
                SoundEngine.PlaySound(icespear);
            }
            else if (fireState == 4)
            {
                type = ProjectileID.Flames;
                damage = 10;
                if (specialAttackTimer == 30)
                {
                    SoundEngine.PlaySound(RiskOfTerrain.GetSound("artiflamethrower/mage_R_start_01").WithVolumeScale(0.7f));
                }
                specialAttackTimer--;
                if (specialAttackTimer == 0)
                {
                    SoundEngine.PlaySound(RiskOfTerrain.GetSound("artiflamethrower/mage_R_end_01").WithVolumeScale(0.7f));
                    specialAttackTimer = 30;
                    fireState = 1;
                }
            }
            else if (fireState == 5)
            {
                if (specialAttackTimer == 30)
                {
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("artinanobomb/mage_m2_charge_elec_v2_", 4));
                }
                specialAttackTimer--;
                if (specialAttackTimer == 0)
                {
                    specialAttackTimer = 30;
                    fireState = 2;
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("artinanobomb/mage_m2_shoot_elec_v2_", 4).WithVolumeScale(0.5f));
                    SoundEngine.PlaySound(RiskOfTerrain.GetSound("artinanobomb/mage_m2_shoot_sweetener").WithVolumeScale(0.5f));
                    type = ModContent.ProjectileType<Nanobomb>();
                }
            }
            else if (fireState == 6)
            {
                type = ModContent.ProjectileType<IceWallSpawner>();
                specialAttackTimer = 30;
                fireState = 3;
            } 
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (fireState == 5 && specialAttackTimer > 0)
            {
                return false;
            }
            if (type == ModContent.ProjectileType<IceWallSpawner>())
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<IceWallSpawner>(), 0, 0, player.whoAmI);
                return false;
            }
            return true;
        }
    }
}