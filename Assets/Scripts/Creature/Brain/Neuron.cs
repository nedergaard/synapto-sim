namespace Assets.Scripts.Creature.Brain
{
    public class Neuron : INeuron
    {
        public byte Id { get; set; }
        public float Bias { get; set; }
    }
}