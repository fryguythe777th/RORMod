using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class StickyExplosivesProj : ModProjectile
    {
        public int AttatchedNPC { get => (int)Projectile.ai[0]; }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            var npcCenter = Main.npc[AttatchedNPC].Center;
            var directionTo = Projectile.DirectionTo(npcCenter);
            Projectile.Center = npcCenter + directionTo * Main.npc[AttatchedNPC].Size / 2f;
            Projectile.rotation = directionTo.ToRotation() - MathHelper.PiOver2;
            if (Projectile.frameCounter > Projectile.timeLeft / 16)
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}