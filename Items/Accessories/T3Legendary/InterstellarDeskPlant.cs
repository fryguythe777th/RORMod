using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Buffs;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class InterstellarDeskPlant : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => NPC.downedBoss3));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<IDPPlant>() && Main.projectile[i].owner == entity.GetProjectileOwnerID())
                {
                    count++;
                }

                if (count == 3)
                {
                    break;
                }
            } 

            if (count < 3)
            {
                Projectile.NewProjectile(entity.GetSource_Accessory(Item), info.Center, new Vector2(0, 3), ModContent.ProjectileType<IDPPlant>(), 0, 0, entity.GetProjectileOwnerID());
            }
        }
    }

    public class IDPPlant : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Projectiles/Accessory/Utility/BustlingFungusProj";
        protected override bool CloneNewInstances => base.CloneNewInstances;
        public SpriteEffects flip = SpriteEffects.None;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
            Projectile.timeLeft = 1200;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(36 * i));
                Dust.NewDust(Projectile.Center, 2, 2, DustID.Clentaminator_Green, velocity.X, velocity.Y);
            }
            SoundEngine.PlaySound(RiskOfTerrain.GetSound("bungus", volume: 0.3f), Projectile.Center);

            if (Main.rand.NextBool())
            {
                flip = SpriteEffects.FlipHorizontally;
            }
        }

        public int playerHealCooldown = 20;

        public override void AI()
        {
            Projectile.velocity.Y *= 1.15f;
            Projectile.scale = MathHelper.Lerp(Projectile.scale, 400f, 0.2f);

            if (Projectile.scale > 0.1f)
            {
                foreach (var c in Helpers.CircularVector((int)(64 * Projectile.scale)))
                {
                    Lighting.AddLight(Projectile.Center + c * Projectile.scale / 2f, new Vector3(0.1f, 1f, 0.2f) * Projectile.scale / 400f * 0.33f);
                }
            }

            if (playerHealCooldown > 0)
            {
                playerHealCooldown--;
            }
            else
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && Main.player[i].Distance(Projectile.Center) < Projectile.scale / 2f && Main.player[i].statLife < Main.player[i].statLifeMax2 &&
                        (!Main.player[i].hostile || !Main.player[Projectile.owner].hostile || Main.player[i].team == Main.player[Projectile.owner].team))
                    {
                        Main.player[i].statLife++;
                    }
                }

                if (NPC.downedPlantBoss)
                {
                    playerHealCooldown = 10;
                }
                else
                {
                    playerHealCooldown = 20;
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.Distance(Projectile.Center) < Projectile.scale / 2f && (npc.friendly || npc.lifeMax <= 5 || npc.damage == 0))
                {
                    if (npc.life < npc.lifeMax)
                    {
                        npc.life++;
                    }
                }
            }

            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.95f, 0.85f) * Projectile.scale / 280f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = new Vector2(0, 3).RotatedBy(MathHelper.ToRadians(36 * i));
                Dust.NewDust(Projectile.Center, 2, 2, DustID.Clentaminator_Green, velocity.X, velocity.Y);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.PrepareDrawnEntityDrawing(Projectile, Main.player[Projectile.owner].ROR().cBungus, null);
            DrawAura(Projectile.Center - Main.screenPosition, Projectile.scale, Projectile.Opacity, ModContent.Request<Texture2D>(Texture + "Aura").Value, TextureAssets.Projectile[Type].Value);

            Asset<Texture2D> plant = ModContent.Request<Texture2D>("RiskOfTerrain/Projectiles/Accessory/Visual/IDPPlantTexture");
            Main.EntitySpriteDraw(plant.Value, Projectile.Center - Main.screenPosition, plant.Value.Bounds, lightColor, Projectile.rotation, plant.Size() * 0.5f, 1f, flip);
            return false;
        }

        public static void DrawAura(Vector2 location, float diameter, float opacity, Texture2D texture, Texture2D circle)
        {
            var origin = texture.Size() / 2f;
            location = location.Floor();
            float scale = diameter / texture.Width;
            opacity = Math.Min(opacity * scale, 1f);

            var color = new Color(255, 255, 255, 0);
            Main.EntitySpriteDraw(texture, location, null,
                color * 0.3f * opacity, 0f, origin, scale, SpriteEffects.None, 0);

            foreach (var c in Helpers.CircularVector(4))
            {
                Main.EntitySpriteDraw(circle, location + c, null,
                    Color.White * opacity, 0f, origin, scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(circle, location, null,
                Color.White * opacity, 0f, origin, scale, SpriteEffects.None, 0);
        }
    }
}