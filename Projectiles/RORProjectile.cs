using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles
{
    internal class RORProjectile : GlobalProjectile
    {
        public override bool PreAI(Projectile projectile)
        {
            if (!projectile.npcProj)
            {
                var player = Main.player[projectile.owner];
                if (player.ROR().accDiosBestFriend != null)
                {
                    player.dead = false;
                }
            }
            return true;
        }
    }
}
