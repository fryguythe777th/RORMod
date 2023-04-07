using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;

namespace RiskOfTerrain.Items.Accessories.T1Common
{
    public class TougherTimes : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedBoss1));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool PreHurt(Player player, RORPlayer ror, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (player.whoAmI == Main.myPlayer && player.RollLuck(10) == 0)
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    var p = RiskOfTerrain.GetPacket(PacketType.TougherTimesDodge);
                    p.Write(player.whoAmI);
                }
                player.SetImmuneTimeForAllTypes(60);
                DoDodgeEffect(player);
                return false;
            }
            return true;
        }

        public static void DoDodgeEffect(Entity entity)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(RiskOfTerrain.GetSound("toughertimes").WithVolumeScale(0.2f), entity.Center);
                int c = CombatText.NewText(new Rectangle((int)entity.position.X + entity.width / 2 - 1, (int)entity.position.Y, 2, 2), new Color(255, 255, 255, 0), 0, false, true);
                if (c != -1 && c != Main.maxCombatText)
                {
                    Main.combatText[c].text = Language.GetTextValue("Mods.RiskOfTerrain.Blocked");
                    Main.combatText[c].rotation = 0f;
                    Main.combatText[c].scale *= 0.8f;
                    Main.combatText[c].alphaDir = 0;
                    Main.combatText[c].alpha = 0.99f;
                    Main.combatText[c].position.X = entity.position.X + entity.width / 2f - FontAssets.CombatText[0].Value.MeasureString(Main.combatText[c].text).X / 2f;
                }
            }
        }
    }
}