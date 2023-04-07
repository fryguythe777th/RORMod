using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    [AutoloadEquip(EquipType.Neck)]
    public class OddlyShapedOpal : ModAccessory
    {
        public const int TimeToActivate = 60 * 7;

        public int shieldActive;
        public bool hideVisual;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public bool ShieldActive()
        {
            return shieldActive > TimeToActivate;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            this.hideVisual = hideVisual;
            shieldActive++;
            if (ShieldActive())
            {
                player.statDefense += 50;
            }
        }

        public override void Hurt(Player player, RORPlayer ror, Player.HurtInfo info)
        {
            if (ShieldActive())
            {
                SoundEngine.PlaySound(SoundID.Item53.WithVolumeScale(1f), player.Center);
            }
            shieldActive = 0;
        }
    }
}