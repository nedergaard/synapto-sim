namespace Assets.Scripts.Creature.Brain
{
    public interface ISynapse
    {
        public float Input { get; set; }

        public float WeightedOutput { get; }
    }
}