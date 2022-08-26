namespace RORMod.Items.Artifacts
{
    public class Dissonance : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.dissonance; set => RORMod.dissonance = value; }
    }
}