using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.NPCs;
using Terraria.Audio;
using RiskOfTerrain.Items.Testing;
using RiskOfTerrain.Projectiles.Misc;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class UnstableTeslaCoil : ModAccessory
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public int zapCooldown = 120;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if(Main.myPlayer != player.whoAmI)
            {
                return;
            }

            if (zapCooldown == 0)
            {
                zapCooldown = 120;

                bool playSound = false;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].Distance(player.Center) < 400 && !Main.npc[i].friendly && Main.npc[i].lifeMax > 5 && Main.npc[i].damage > 0 && Main.npc[i].active)
                    {
                        Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<LightningEffectProj>(), 0, 0, ai0: i, Owner: player.whoAmI);

                        NPC.HitInfo hit = new NPC.HitInfo
                        {
                            DamageType = DamageClass.Default,
                            SourceDamage = 20,
                            Damage = 20,
                            Crit = false,
                            Knockback = 0f,
                            HitDirection = 0
                        };
                        Main.npc[i].StrikeNPC(hit);
                        NetMessage.SendStrikeNPC(Main.npc[i], hit);

                        playSound = true;
                    }
                }

                if (Main.netMode != NetmodeID.Server && playSound)
                {
                    SoundEngine.PlaySound(RiskOfTerrain.GetSounds("ukulele/item_proc_chain_lightning_", 4).WithVolumeScale(0.4f), player.Center);
                }
            }
            else
            {
                zapCooldown--;
            }
        }
    }
}