namespace Assets.Scripts.Creature.Brain
{
    public class Neuron : INeuron
    {
        public byte Id { get; set; }
        public float Bias { get; set; }

        #region Implementation of INeuron

        /// <inheritdoc />
        public float Output { get; }

        /// <inheritdoc />
        public void FeedForward()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}