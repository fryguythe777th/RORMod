using Microsoft.Xna.Framework;
using RiskOfTerrain.Items.Accessories.Aspects;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class OverloadingElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.BlueDye);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
            if (active)
            {
                npc.ROR().maxShield += 1f;
                npc.ROR().isAShielder = true;
            }
        }

        public override bool CanRoll(NPC npc)
        {
            return true;
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.npcSlots *= 4f;
            npc.value *= 2;
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (active)
            {
                //something's outside of an index
                int p = Projectile.NewProjectile(npc.GetSource_FromThis(), target.Center + new Vector2(Main.rand.Next(0, 16), Main.rand.Next(0, 16)), Vector2.Zero, ModContent.ProjectileType<OverloadingBomb>(), 0, 0);
                Main.projectile[p].ROR().spawnedFromElite = true;
                Main.projectile[p].friendly = false;
                Main.projectile[p].hostile = true;
                Main.projectile[p].ai[0] = target.whoAmI;
                Main.projectile[p].ai[1] = 1;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (active)
            {
                int rollNumber = npc.boss ? 1000 : 4000;
                if (Main.player[Player.FindClosest(npc.Center, 500, 500)].RollLuck(rollNumber) == 0)
                {
                    int i = Item.NewItem(npc.GetSource_GiftOrReward(), npc.Center, ModContent.ItemType<OverloadingAspect>());
                    Main.item[i].velocity = new Vector2(0, -4);
                }
            }
        }
    }
}