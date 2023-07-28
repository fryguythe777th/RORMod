using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public class MalachiteElite : EliteNPCBase
    {
        public override ArmorShaderData Shader => GameShaders.Armor.GetShaderFromItemId(ItemID.GreenandBlackDye);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void AI(NPC npc)
        {
        }

        public override bool CanRoll(NPC npc)
        {
            return Main.hardMode;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);
        }

        public override void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 4f);
            npc.life = (int)(npc.life * 4f);
            npc.npcSlots *= 16f;
            npc.value *= 4;
            SoundEngine.PlaySound(new SoundStyle("RiskOfTerrain/Assets/Sounds/malachitegong"));
        }
    }
}