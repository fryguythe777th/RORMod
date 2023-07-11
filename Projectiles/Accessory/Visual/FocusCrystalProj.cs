using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Visual
{
    public class FocusCrystalProj : ModProjectile
    {
        public bool accessoryVisible;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
            Projectile.timeLeft = 20;
        }

        public override void AI()
        {
            bool active = true;
            if (Projectile.numUpdates == -1)
            {
                active = accessoryVisible;
                accessoryVisible = false;
            }

            if (!active)
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 0.2f);
                if (Projectile.scale < 0.1f)
                {
                    return;
                }
            }
            Projectile.timeLeft = 2;

            if (Projectile.scale > 0.1f)
            {
                foreach (var c in Helpers.CircularVector((int)(64 * Projectile.scale)))
                {
                    Lighting.AddLight(Projectile.Center + c * Projectile.scale / 2f, new Vector3(1f, 0.1f, 0.1f) * Projectile.scale / 400f * 0.33f);
                }
            }

            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.1f, 0.1f) * Projectile.scale / 280f);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.PrepareDrawnEntityDrawing(Projectile, Main.player[Projectile.owner].ROR().cFocusCrystal, null);
            BustlingFungusProj.DrawAura(Projectile.Center - Main.screenPosition, Projectile.scale, Projectile.Opacity * 0.4f, ModContent.Request<Texture2D>(Texture + "Aura").Value, TextureAssets.Projectile[Type].Value);
            return false;
        }
    }
}