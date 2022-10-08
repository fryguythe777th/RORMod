using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites
{
    public abstract class EliteNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        public virtual string Prefix { get => Language.GetTextValue($"Mods.{Mod.Name}.{Name}"); }

        public virtual ArmorShaderData Shader { get; }

        protected bool active;
        public virtual bool Active { get => active; set => active = value; }

        public byte NetID { get; private set; }

        public override void SetStaticDefaults()
        {
            NetID = (byte)RORNPC.RegisteredElites.Count;
            RORNPC.RegisteredElites.Add(this);
        }

        public virtual bool CanRoll(NPC npc)
        {
            return true;
        }

        public virtual int RollChance(NPC npc)
        {
            return (Main.hardMode ? 25 : 50) * (!Main.expertMode ? 2 : 1);
        }

        public void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (int)(npc.lifeMax * 4f);
            npc.life = (int)(npc.life * 4f);
            npc.damage = (int)(npc.damage * 2f);
            npc.npcSlots *= 4f;
        }
    }
}