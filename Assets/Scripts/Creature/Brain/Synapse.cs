namespace Assets.Scripts.Creature.Brain
{
    public class Synapse : ISynapse
    {
        public Synapse(float weight)
        {
            Weight = weight;
        }

        #region Implementation of ISynapse

        public float Weight { get; }

        /// <inheritdoc />
        public float Input { get; set; }

        /// <inheritdoc />
        public float WeightedOutput => Weight * Input;

        #endregion
    }
}