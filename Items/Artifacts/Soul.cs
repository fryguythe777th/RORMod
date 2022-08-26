namespace RORMod.Items.Artifacts
{
    public class Soul : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.soul; set => RORMod.soul = value; }
    }
}