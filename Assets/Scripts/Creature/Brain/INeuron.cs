namespace Assets.Scripts.Creature.Brain
{
    public interface INeuron
    {
        /// <summary>
        /// Read inputs and update Output.
        /// </summary>
        public void FeedForward();
    }
}