namespace RORMod.Items.Artifacts
{
    public class Vengeance : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.vengeance; set => RORMod.vengeance = value; }

        public override bool unimplemented => true;
    }
}