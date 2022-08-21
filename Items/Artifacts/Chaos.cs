using Microsoft.Xna.Framework;
using Terraria;

namespace ROR2Artifacts.Items.Artifacts
{
    public class Chaos : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.ChaosActive; set => ROR2Artifacts.ChaosActive = value; }

        public override bool? UseItem(Player player)
        {
            base.UseItem(player);
            if (Main.myPlayer == player.whoAmI && ROR2Artifacts.ChaosActive)
                ROR2Artifacts.Broadcast("Announcements.ChaosPVP", Color.Red);
            return true;
        }
    }
}