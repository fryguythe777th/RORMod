namespace RORMod.Items.Artifacts
{
    public class Enigma : BaseArtifact
    {
        public override bool ActiveFlag { get => RORMod.enigma; set => RORMod.enigma = value; }
    }
}