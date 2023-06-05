namespace Assets.Scripts.Creature.Brain
{
    public interface INeuron
    {
        /// <summary>
        /// Current output value.
        /// </summary>
        public float Output { get; }

        /// <summary>
        /// Read inputs and update Output.
        /// </summary>
        public void FeedForward();
    }
}