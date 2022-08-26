namespace RORMod.Items.Artifacts
{
    public class Command : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.command; set => RORMod.command = value; }

        public override bool unimplemented => true;
    }
}