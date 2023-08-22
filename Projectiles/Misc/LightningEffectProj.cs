using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using RiskOfTerrain.NPCs;

namespace RiskOfTerrain.Projectiles.Misc
{
    /**
     * Should probable change this so it can handle drawing to multiple monsters, now we might get out of projectiles if we draw to much
     * 
     */
    public class LightningEffectProj: ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.hide = false;

            Projectile.timeLeft = 2;
            Projectile.alpha = 0;
            Projectile.damage = 0;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }


        public override string Texture => "RiskOfTerrain/Projectiles/Accessory/Damaging/UkuleleLightning";


        public override void PostDraw(Color lightColor)
        {
             var npc = Main.npc[(int)Projectile.ai[0]];
            var player = Main.player[Projectile.owner];

            if (npc.active)
            {
                for (int j = 0; j < RORNPC.Distance(player, npc); j++)
                {
                    float angle = player.Center.AngleTo(npc.Center) - MathHelper.ToRadians(90);

                    Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
                    var pos = player.Center + new Vector2(0, j).RotatedBy(angle);
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition, Color.LightBlue);
                }
            }
        }
    }
}
