﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Common {
    public static class Helper {
        public static bool ConsumeItemInInvOrVoidBag(this Player player, int itemType) {
            if (player.ConsumeItem(itemType)) {
                return true;
            }

            if (!player.HasItem(ItemID.VoidLens)) {
                return false;
            }

            for (int i = 0; i < Chest.maxItems; i++) {
                if (!player.bank4.item[i].IsAir && player.bank4.item[i].type == itemType) {
                    if (ItemLoader.ConsumeItem(player.bank4.item[i], player)) {
                        player.bank4.item[i].stack--;
                    }
                    if (player.bank4.item[i].stack <= 0) {
                        player.bank4.item[i].TurnToAir();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
