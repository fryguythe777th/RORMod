namespace RORMod.Items.Artifacts
{
    public class Evolution : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.evolution; set => RORMod.evolution = value; }

        public override bool unimplemented => true;
    }
}