using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Graphics;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class Headset : ModAccessory
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public bool StompReady = true;
        public bool StompActive = false;
        public int StompCooldown = 600;
        public Vector2 StompStart;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;

            if (StompReady)
            {
                player.jumpBoost = true;
                player.jumpSpeedBoost += 3f;

                if (RORKeybinds.HeadsetKey.JustPressed && player.TouchedTiles.Count == 0)
                {
                    StompReady = false;
                    StompActive = true;
                    StompStart = player.Center;
                }
            }

            if (StompActive)
            {
                player.gravity += 7f;

                if (player.TouchedTiles.Count > 0)
                {
                    ROREffects.Shake.Set((player.Center.Y - StompStart.Y) / 10);
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<HeadsetSmash>(), (int)((player.Center.Y - StompStart.Y) / 5), 4, player.whoAmI);
                    StompActive = false;
                }
            }

            if (!StompReady)
            {
                StompCooldown--;

                if (StompCooldown == 0)
                {
                    StompReady = true;
                    StompCooldown = 600;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable",
                Language.GetTextValueWith("Mods.RiskOfTerrain.Items.Headset.KeybindTooltip", new { Keybind = $"[{Helpers.GetKeyName(RORKeybinds.HeadsetKey)}]" })));
        }
    }

    public class HeadsetSmash : ModProjectile
    {
        public override string Texture => "RiskOfTerrain/Items/Accessories/T3Legendary/Headset";

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.damage = 1;
            Projectile.scale = 2f;
            Projectile.Opacity = 0;
            Projectile.timeLeft = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.knockBack = 4;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.penetrate = -1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return false;
        }
    }
}