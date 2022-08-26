namespace RORMod.Items.Artifacts
{
    public class Swarms : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.swarms; set => RORMod.swarms = value; }

        public override bool unimplemented => true;
    }
}