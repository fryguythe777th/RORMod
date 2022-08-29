using RORMod.Content.Artifacts;

namespace RORMod.Items.Artifacts
{
    public class Soul : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.soul; set => ArtifactSystem.soul = value; }
    }
}