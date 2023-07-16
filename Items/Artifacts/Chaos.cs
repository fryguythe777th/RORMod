using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Artifacts;
using Terraria;

namespace RiskOfTerrain.Items.Artifacts
{
    public class Chaos : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.chaos; set => ArtifactSystem.chaos = value; }

        //public override bool? UseItem(Player player)
        //{
        //    base.UseItem(player);
        //    if (Main.myPlayer == player.whoAmI && ArtifactSystem.chaos)
        //        RiskOfTerrain.BroadcastMessage("Announcements.ChaosPVP", Color.Red);
        //    return true;
        //}

        public override bool unimplemented => true;
    }
}