using Microsoft.Xna.Framework;
using RORMod.Buffs.Debuff;
using RORMod.Common.Networking;
using RORMod.Projectiles.Misc;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.NPCs
{
    public class RORNPC : GlobalNPC
    {
        public byte bleedingStacks;
        public bool bleedShatterspleen;

        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff<BleedingDebuff>())
            {
                bleedShatterspleen = false;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (npc.netUpdate)
                Sync(npc.whoAmI);

            if (Main.dedServ)
                return;

            if (npc.HasBuff(ModContent.BuffType<BleedingDebuff>()) && Main.rand.NextBool(5))
            {
                Dust.NewDust(npc.position, 4, 4, DustID.Blood, Main.rand.NextBool().ToDirectionInt(), Main.rand.NextBool().ToDirectionInt(), 0, Scale: 1.5f);
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            UpdateDebuffStack(npc, npc.HasBuff<BleedingDebuff>(), ref bleedingStacks, ref damage, 20, 16f);
        }
        public void UpdateDebuffStack(NPC npc, bool has, ref byte stacks, ref int damageNumbers, byte cap = 20, float dotMultiplier = 1f)
        {
            if (!has)
            {
                stacks = 0;
                return;
            }

            stacks = Math.Min(stacks, cap);
            int dot = (int)(stacks * dotMultiplier);

            if (dot >= 0)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= dot;
                if (damageNumbers < dot)
                    damageNumbers = dot / 8;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            ModifiyHit(npc, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ModifiyHit(npc, ref damage, ref knockback, ref crit);
        }

        public void ModifiyHit(NPC npc, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff<DeathMarkDebuff>())
                damage += (int)(damage * 0.1f);
        }

        public override void OnKill(NPC npc)
        {
            var closest = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
            if (bleedShatterspleen)
            {
                Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, npc.DirectionFrom(closest.Center) * 0.1f, 
                    ModContent.ProjectileType<ShatterspleenExplosion>(), npc.lifeMax / 4, 6f, closest.whoAmI);
            }
            if (closest.ROR().accMonsterTooth)
            {
                Projectile.NewProjectile(npc.GetSource_FromThis(), new Vector2(npc.Center.X, npc.position.Y), new Vector2(0, 1), ModContent.ProjectileType<HealingOrb>(), 0, 0, closest.whoAmI);
            }
        }

        public void Send(int whoAmI, BinaryWriter writer)
        {
            writer.Write(bleedingStacks);

            var bb = new BitsByte(bleedShatterspleen);
            writer.Write(bb);
        }

        public void Receive(int whoAmI, BinaryReader reader)
        {
            bleedingStacks = reader.ReadByte();

            var bb = (BitsByte)reader.ReadByte();
            bleedShatterspleen = bb[0];
        }

        public static void Sync(int npc)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;

            if (Main.npc[npc].TryGetGlobalNPC<RORNPC>(out var aequus))
            {
                var p = RORMod.GetPacket(PacketType.SyncRORNPC);
                aequus.Send(npc, p);
                p.Send();
            }
        }
    }
}