using Microsoft.Xna.Framework;
using RORMod.Content.Artifacts;
using Terraria;

namespace RORMod.Items.Artifacts
{
    public class Chaos : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.chaos; set => ArtifactSystem.chaos = value; }

        public override bool? UseItem(Player player)
        {
            base.UseItem(player);
            if (Main.myPlayer == player.whoAmI && ArtifactSystem.chaos)
                RORMod.BroadcastMessage("Announcements.ChaosPVP", Color.Red);
            return true;
        }
    }
}