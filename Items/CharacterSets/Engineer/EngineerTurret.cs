using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Items.Accessories.T1Common;
using RiskOfTerrain.Items.Accessories.T2Uncommon;
using RiskOfTerrain.Items.Accessories.T3Legendary;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Engineer
{
    public class EngineerTurret : ModProjectile
    {
        private static Asset<Texture2D> baseTexture;
        private static Asset<Texture2D> gunTexture;

        public override void Load()
        {
            baseTexture = ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Engineer/EngineerTurretBase");
            gunTexture = ModContent.Request<Texture2D>("RiskOfTerrain/Items/CharacterSets/Engineer/EngineerTurret");
        }

        public override void Unload()
        {
            baseTexture = null;
            gunTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.penetrate = -1;
            Projectile.sentry = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 36000;
        }

        public List<Item> items = new List<Item>();

        public SpriteEffects spriteEffects;

        public int currentShootCooldown;
        public int shootCooldownCap;

        public int shurikenCharges;
        public int shurikenRechargeTime;

        public int zapCooldown = 0;

        public int fishingPower;

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 7; i++)
            {
                items.Add(player.armor[i + 3]);
            }

            if (player.fishingSkill > 0)
            {
                AdvancedPopupRequest fishPopup = new AdvancedPopupRequest();
                fishPopup.Text = ("Fishing Power: " + player.fishingSkill);
                fishPopup.Color = Color.LightBlue;
                fishPopup.DurationInFrames = 180;
                PopupText.NewText(fishPopup, new Vector2(Projectile.Center.X, Projectile.Center.Y + 20));
            }
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            shootCooldownCap = 20;

            for (int i = 0; i < 7; i++)
            {
                Item item = player.armor[i + 3];

                shootCooldownCap = (int)(20 / player.GetAttackSpeed<GenericDamageClass>());
            }

            return true;
        }

        public override void AI()
        {
            while (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.position.Y++;
            }
            Player player = Main.player[Projectile.owner];
            int target = Projectile.FindTargetWithLineOfSight(800);

            if (target == -1)
            {
                float rot = (player.Center - Projectile.Center).ToRotation();
                Projectile.rotation = rot + MathHelper.Pi;
                currentShootCooldown = shootCooldownCap;

                if (player.Center.X >= Projectile.Center.X) //it needs to flip vertically in truth
                {
                    spriteEffects = SpriteEffects.FlipVertically;
                }
                else
                {
                    spriteEffects = SpriteEffects.None;
                }
            }
            else
            {
                NPC npc = Main.npc[target];

                float rot = (npc.Center - Projectile.Center).ToRotation();
                Projectile.rotation = rot + MathHelper.Pi;

                if (npc.Center.X >= Projectile.Center.X) //flip 
                {
                    spriteEffects = SpriteEffects.FlipVertically;
                }
                else
                {
                    spriteEffects = SpriteEffects.None;
                }

                if (currentShootCooldown == 0)
                {
                    currentShootCooldown = shootCooldownCap;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(16, 0).RotatedBy(rot), ModContent.ProjectileType<EngineerTurretBullet>(), 10, 1, Projectile.owner);

                    shurikenCharges = Math.Min(shurikenCharges, 3);
                    if (shurikenCharges > 0 && (shurikenRechargeTime > 20 || shurikenCharges == 3))
                    {
                        shurikenCharges--;
                        shurikenRechargeTime = 0;
                        var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(20, 0).RotatedBy(rot),
                            ModContent.ProjectileType<ReloadingShurikenProj>(), 20, 1f, owner: Projectile.owner);
                        p.DamageType = DamageClass.Summon;
                    }
                }
                else
                {
                    currentShootCooldown--;
                }
            }

            bool hasShuriken = false;

            for (int i = 0; i < 7; i++)
            {
                Item item = player.armor[i + 3];

                if (item.type == ModContent.ItemType<BustlingFungus>())
                {
                    bool bungusSpotted = false;
                    for (int j = 0; j < Main.maxProjectiles; j++)
                    {
                        if (Main.projectile[j].active && Main.projectile[j].type == ModContent.ProjectileType<ProjOwnedBustlingFungusProj>())
                        {
                            bungusSpotted = true;
                            continue;
                        }
                    }

                    if (!bungusSpotted)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ProjOwnedBustlingFungusProj>(), 0, 0, Projectile.whoAmI);
                    }
                }
                if (item.type == ModContent.ItemType<ReloadingShurikens>())
                {
                    hasShuriken = true;
                }
                if (item.type == ModContent.ItemType<UnstableTeslaCoil>())
                {
                    if (zapCooldown == 0)
                    {
                        zapCooldown = 120;

                        bool playSound = false;

                        for (int j = 0; j < Main.maxNPCs; j++)
                        {
                            if (Main.npc[j].Distance(Projectile.Center) < 400 && !Main.npc[j].friendly && Main.npc[j].lifeMax > 5 && Main.npc[j].damage > 0 && Main.npc[j].active)
                            {
                                RORNPC.TeslaLightning(Projectile, Main.npc[j], 20);
                                playSound = true;
                            }
                        }

                        if (Main.netMode != NetmodeID.Server && playSound)
                        {
                            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("ukulele/item_proc_chain_lightning_", 4).WithVolumeScale(0.4f), Projectile.Center);
                        }
                    }
                    else
                    {
                        zapCooldown--;
                    }
                }
            }

            if (hasShuriken)
            {
                if (shurikenCharges < 3)
                {
                    shurikenRechargeTime++;
                    if (shurikenRechargeTime > 120)
                    {
                        shurikenRechargeTime = 0;
                        shurikenCharges++;
                    }
                }
            }
            else
            {
                shurikenCharges = 0;
                shurikenRechargeTime = 0;
            }
        }

        public override void PostDraw(Microsoft.Xna.Framework.Color lightColor)
        {
            Main.EntitySpriteDraw(gunTexture.Value, Projectile.Center - Main.screenPosition, gunTexture.Value.Bounds, lightColor, Projectile.rotation, gunTexture.Size() * 0.5f, 1f, spriteEffects, 0);
            Main.EntitySpriteDraw(baseTexture.Value, Projectile.Center - Main.screenPosition, baseTexture.Value.Bounds, lightColor, 0, baseTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
        }
    }

    public class EngineerTurretBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 20;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 0;
            Projectile.aiStyle = -1;
        }

        public float damage;
        public float critChance;
        public float critModifier;

        public override bool PreAI()
        {
            damage = 10;

            critChance = 1f;

            critModifier = 2f;

            return true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
    }
}