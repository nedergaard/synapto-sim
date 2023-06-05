using Assets.Scripts.Creature.Sense;

namespace Assets.Scripts.Creature.Brain
{
    public class SenseNeuron : INeuron
    {
        private readonly ISense _sense;
        private readonly float _rangeFactor;
        private readonly float _inputMedian;

        public SenseNeuron(ISense sense)
        {
            _sense = sense;

            const float outputRange = 2; // -1 to +1
            var inputRange = sense.MaximumSensoryInput - sense.MinimumSensoryInput;
            _rangeFactor = outputRange / inputRange;
            _inputMedian = sense.MinimumSensoryInput + inputRange / 2f;
        }

        #region Implementation of INeuron

        /// <inheritdoc />
        public float Output { get; private set; }

        /// <inheritdoc />
        public void FeedForward()
        {
            Output = Normalize(_sense.GetSensoryInput());
        }

        #endregion

        private float Normalize(float input) => (input - _inputMedian) * _rangeFactor;
    }
}
