using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Kin : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.kin; set => ArtifactSystem.kin = value; }

        public override bool unimplemented => true;
    }
}