using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.T2Uncommon
{
    public class LeptonDaisy : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.GreenTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2);
        }

        public int HealCooldown = 1800; // 30 seconds 

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NPC.AnyDanger(false, true))
            {
                if (HealCooldown > 0)
                {
                    HealCooldown--;
                }

                if (HealCooldown == 0)
                {
                    HealCooldown = 1800;

                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Main.player[i].Heal((int)(Main.player[i].statLifeMax * 0.3));
                    }

                    if (!Main.dedServ)
                    {
                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            for (int j = 0; j < Main.rand.Next(8, 13); j++)
                            {
                                int dimensions = Main.rand.Next(1, 2);
                                Dust.NewDust(Main.player[j].Center, dimensions, dimensions, DustID.Clentaminator_Green, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
                            }
                        }
                    }
                }
            }
        }
    }
}