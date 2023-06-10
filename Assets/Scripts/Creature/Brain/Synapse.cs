namespace Assets.Scripts.Creature.Brain
{
    public class Synapse
        : ISynapse
    {
        public INeuron OutputNeuron { get; set; }
        public float Weight { get; set; }

        #region Implementation of ISynapse

        /// <inheritdoc />
        public float Input { get; set; }

        /// <inheritdoc />
        public float WeightedOutput { get; }

        #endregion
    }
}