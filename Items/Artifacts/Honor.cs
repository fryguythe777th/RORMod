namespace RORMod.Items.Artifacts
{
    public class Honor : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.honor; set => RORMod.honor = value; }

        public override bool unimplemented => true;
    }
}