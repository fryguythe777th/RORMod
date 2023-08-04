using Microsoft.Xna.Framework;
using RiskOfTerrain.Items.Accessories.Aspects;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class TrollElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.ReflectiveGoldDye);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
        }

        public override bool CanRoll(NPC npc)
        {
            return false;
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1f);
            npc.life = (int)(npc.life * 1f);
            npc.npcSlots *= 1f;
            npc.value *= 1;
        }

        public override void OnKill(NPC npc)
        {
            if (active)
            {
                int rollNumber = npc.boss ? 1 : 4;
                if (Main.player[Player.FindClosest(npc.Center, 500, 500)].RollLuck(rollNumber) == 0)
                {
                    int i = Item.NewItem(npc.GetSource_GiftOrReward(), npc.Center, ModContent.ItemType<TrollAspect>());
                    Main.item[i].velocity = new Vector2(0, -4);
                }
            }
        }
    }
}