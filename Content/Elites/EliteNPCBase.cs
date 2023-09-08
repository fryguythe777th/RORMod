using RiskOfTerrain.NPCs;
using System;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RiskOfTerrain.Content.Elites
{
    public abstract class EliteNPCBase : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        int myNPCId;
        public override GlobalNPC Clone(NPC from, NPC to)
        {

            var inst = base.Clone(from, to) as EliteNPCBase;
            inst.myNPCId = to.whoAmI;
            return inst;
        }

        public virtual string Prefix { get => Language.GetTextValue($"Mods.{Mod.Name}.{Name}"); }

        public virtual ArmorShaderData Shader { get; }

        protected bool active = false;

        public virtual bool Active {
            get => active;
            set {
                if( active && !value)
                {
                    throw new Exception("We can currently not disable Elite");
                }
                if(!active && value )
                {
                    this.OnBecomeElite(Main.npc[this.myNPCId]);
                }
                active = value;
            }
        }

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

        public virtual void OnBecomeElite(NPC npc)
        {
            npc.lifeMax = (npc.lifeMax * 2);
            npc.life = (npc.life * 2);
            npc.npcSlots *= 4f;
            npc.value *= 2;
            npc.netUpdate = true;
        }

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(npc, bitWriter, binaryWriter);

            binaryWriter.Write(npc.GetGlobalNPC(this).Active);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            base.ReceiveExtraAI(npc, bitReader, binaryReader);
            var active = binaryReader.ReadBoolean();
            npc.GetGlobalNPC(this).myNPCId = npc.whoAmI;
            npc.GetGlobalNPC(this).Active = active;
        }
    }
}