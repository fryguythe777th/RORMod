namespace RORMod.Items.Artifacts
{
    public class Glass : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.glass; set => RORMod.glass = value; }
    }
}