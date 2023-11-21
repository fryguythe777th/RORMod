using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Front)]
    public class CautiousSlug : ModAccessory
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public int glubbyActive;
        public bool hideVisual;

        public override void Load()
        {
            EquipLoader.AddEquipTexture(Mod, Texture + "_Front_Hide", EquipType.Front, name: "CautiousSlug_Hide");
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);

            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<CautiousSlugCritter>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.noUseGraphic = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            this.hideVisual = hideVisual;
        }

        public override void UpdateLifeRegeneration(EntityInfo entity)
        {
            if (glubbyActive > 120)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    var slot = (sbyte)EquipLoader.GetEquipSlot(Mod, "CautiousSlug", EquipType.Front);
                    if (Item.frontSlot != slot)
                    {
                        Item.frontSlot = slot;
                        SoundEngine.PlaySound(RiskOfTerrain.GetSound("glubby").WithVolumeScale(0.4f), entity.entity.Center);
                    }
                }
                if (entity.InDanger())
                {
                    glubbyActive = 0;
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (!hideVisual)
                            SoundEngine.PlaySound(RiskOfTerrain.GetSound("glubbyhide").WithVolumeScale(0.4f));
                        Item.frontSlot = (sbyte)EquipLoader.GetEquipSlot(Mod, "CautiousSlug_Hide", EquipType.Front);
                    }
                }
                int regen = 2;
                if (entity.entity is Player player && player.lifeRegenTime < 3000)
                {
                    player.lifeRegenTime += 2;
                    regen += (int)Math.Min(player.lifeRegenTime, (int)3000) / 300;
                }
                entity.AddLifeRegen(regen);
                return;
            }

            if (!entity.InDanger())
            {
                glubbyActive++;
            }
        }
    }
}