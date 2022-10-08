using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles
{
    public class RORProjectile : GlobalProjectile
    {
        public float procRate = 1f;

        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.friendly && !projectile.npcProj)
            {
                Main.player[projectile.owner].ROR().procRate = procRate;
            }
        }
    }
}