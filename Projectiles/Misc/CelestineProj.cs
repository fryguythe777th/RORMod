using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class CelestineProj : ModProjectile
    {
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
            if (Projectile.scale > 0.1f)
            {
                foreach (var c in Helpers.CircularVector((int)(64 * Projectile.scale)))
                {
                    Lighting.AddLight(Projectile.Center + c * Projectile.scale / 2f, new Vector3(1f, 0.1f, 0.1f) * Projectile.scale / 400f * 0.33f);
                }
            }

            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.95f, 0.85f) * Projectile.scale / 280f);

            if (!Main.npc[(int)Projectile.ai[0]].active)
            {
                Projectile.Kill();
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Main.instance.PrepareDrawnEntityDrawing(Projectile, Main.player[Projectile.owner].ROR().cFocusCrystal);
            BustlingFungusProj.DrawAura(Projectile.Center - Main.screenPosition, Projectile.scale, Projectile.Opacity * 0.4f, ModContent.Request<Texture2D>(Texture + "Aura").Value, TextureAssets.Projectile[Type].Value);
            return false;
        }
    }
}