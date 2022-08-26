using Microsoft.Xna.Framework;
using Terraria;

namespace RORMod.Items.Artifacts
{
    public class Chaos : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.chaos; set => RORMod.chaos = value; }

        public override bool? UseItem(Player player)
        {
            base.UseItem(player);
            if (Main.myPlayer == player.whoAmI && RORMod.chaos)
                RORMod.BroadcastMessage("Announcements.ChaosPVP", Color.Red);
            return true;
        }
    }
}