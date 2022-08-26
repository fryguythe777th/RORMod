namespace RORMod.Items.Artifacts
{
    public class Metamorphosis : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.metamorphosis; set => RORMod.metamorphosis = value; }

        public override bool unimplemented => true;
    }
}