using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace RORMod.Content.Elites
{
    public class VoidtouchedElite : EliteNPC
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.PurpleDye);

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