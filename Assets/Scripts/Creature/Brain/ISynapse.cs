namespace Assets.Scripts.Creature.Brain
{
    public interface ISynapse
    {
        float Weight { get; }

        public float Input { get; set; }

        public float WeightedOutput { get; }
    }
}