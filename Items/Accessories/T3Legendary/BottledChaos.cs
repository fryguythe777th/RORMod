using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class BottledChaos : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void OnUseItem(EntityInfo entity, Item item)
        {
            if (((item.buffType != 0 && item.buffTime > 0 && !entity.HasBuff(item.buffType)) || item.potion) && item.consumable)
            {
                int rand = Main.rand.Next(0, 77);
                int buffChoice = (int)RORBuff.BottledChaosOptions.GetValue(rand);
                entity.AddBuff(buffChoice, 600);
                Projectile.NewProjectile(entity.GetSource_Accessory(Item), entity.Center, new Vector2(0, -3), ModContent.ProjectileType<BottledChaosIndicator>(), 0, 0, entity.GetProjectileOwnerID(), rand);
            }
        }
    }

    public class BottledChaosIndicator : ModProjectile
    {
        private static Asset<Texture2D> texture;

        public override void Load()
        {
            texture = ModContent.Request<Texture2D>("RiskOfTerrain/Items/Accessories/T1Common/BustlingFungus");
        }

        public override void Unload()
        {
            texture = null;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 255;
        }

        public override string Texture => "RiskOfTerrain/Items/Accessories/T1Common/BustlingFungus";

        public override bool PreDraw(ref Color lightColor)
        {
            texture = (Asset<Texture2D>)RORBuff.BottledChaosTextures.GetValue((int)Projectile.ai[0]);

            Color color = new Color(lightColor.R, lightColor.G, lightColor.B, Projectile.alpha);

            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, texture.Value.Bounds, color, Projectile.rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.97f;
            Projectile.alpha = Projectile.timeLeft;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 7; i++)
            {
                int size = Main.rand.Next(1, 3);
                Dust.NewDust(Projectile.Center, size, size, DustID.GemSapphire, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
            }
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);
        }
    }
}