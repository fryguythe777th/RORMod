using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace RiskOfTerrain.Content.Elites
{
    public class CelestineElite : EliteNPC
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.FogboundDye);

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