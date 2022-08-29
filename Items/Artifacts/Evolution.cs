using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Evolution : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.evolution; set => ArtifactSystem.evolution = value; }

        public override bool unimplemented => true;
    }
}