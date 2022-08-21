namespace ROR2Artifacts.Items.Artifacts
{
    public class Evolution : BaseArtifact
    {
        public override bool ActiveFlag { get => ROR2Artifacts.EvolutionActive; set => ROR2Artifacts.EvolutionActive = value; }

        public override bool unimplemented => true;
    }
}