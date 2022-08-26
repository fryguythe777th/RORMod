namespace RORMod.Items.Artifacts
{
    public class Death : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.death; set => RORMod.death = value; }
    }
}