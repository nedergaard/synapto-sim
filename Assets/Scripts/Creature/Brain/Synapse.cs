namespace Assets.Scripts.Creature.Brain
{
    public class Synapse
        : ISynapse
    {
        public INeuron InputNeuron { get; set; }
        public INeuron OutputNeuron { get; set; }
        public float Weight { get; set; }
    }
}