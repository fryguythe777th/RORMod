using RiskOfTerrain.Content.Accessories;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Shoes)]
    public class PaulsGoatHoof : ModAccessory
    {
        public bool hideVisual;
        public bool playedSoundForThisFrame;

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
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.2f;
            player.runAcceleration *= 1.5f;
            player.jumpSpeedBoost += 1f;
            player.ROR().bootSpeed += 2f;
            this.hideVisual = hideVisual;
        }

        public override void PostUpdate(EntityInfo entity)
        {
            if (hideVisual)
                return;

            if (entity.entity is Player player)
            {
                int legFrame = player.legFrame.Y / 56;
                if (legFrame == 5 || legFrame == 10 || legFrame == 17)
                {
                    if (!playedSoundForThisFrame)
                        SoundEngine.PlaySound(RiskOfTerrain.GetSounds("goathoof/step_", 7, 0.33f, 0f, 0.1f));
                    playedSoundForThisFrame = true;
                    return;
                }
                playedSoundForThisFrame = false;
            }
        }
    }
}