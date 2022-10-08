using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace RiskOfTerrain.Content.Elites
{
    public class MendingElite : EliteNPC
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.GreenDye);

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
    }
}