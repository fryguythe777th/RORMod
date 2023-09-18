using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class HopooFeather : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.GreenTier.Add((Type, () => NPC.downedBoss3));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.jumpSpeedBoost *= 1.5f;
            player.jumpBoost = true;
            player.GetJumpState<HopooDoubleJump>().Enable();
        }
    }

    public class HopooDoubleJump : ExtraJump
    {
        public override Position GetDefaultPosition() => new After(CloudInABottle);

        public override float GetDurationMultiplier(Player player)
        {
            return 1f;
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;

            offsetY -= 16;

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position + new Vector2(-34f, offsetY), 102, 32, DustID.Cloud, -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 100, Color.White, 1.5f);
                dust.velocity = dust.velocity * 0.5f - player.velocity * new Vector2(0.1f, 0.3f);
            }

            SpawnCloudPoof(player, player.Top + new Vector2(-16f, offsetY));
            SpawnCloudPoof(player, player.position + new Vector2(-36f, offsetY));
            SpawnCloudPoof(player, player.TopRight + new Vector2(4f, offsetY));
        }

        private static void SpawnCloudPoof(Player player, Vector2 position)
        {
            Gore gore = Gore.NewGoreDirect(player.GetSource_FromThis(), position, -player.velocity, Main.rand.Next(11, 14));
            gore.velocity.X = gore.velocity.X * 0.1f - player.velocity.X * 0.1f;
            gore.velocity.Y = gore.velocity.Y * 0.1f - player.velocity.Y * 0.05f;
        }
    }
}