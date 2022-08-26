namespace RORMod.Items.Artifacts
{
    public class Kin : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.kin; set => RORMod.kin = value; }

        public override bool unimplemented => true;
    }
}