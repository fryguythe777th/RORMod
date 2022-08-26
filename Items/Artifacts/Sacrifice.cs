namespace RORMod.Items.Artifacts
{
    public class Sacrifice : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.sacrifice; set => RORMod.sacrifice = value; }

        public override bool unimplemented => true;
    }
}