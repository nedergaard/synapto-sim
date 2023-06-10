using System.Collections.Generic;
using Assets.Scripts.Creature.Sense;

namespace Assets.Scripts.Creature.Brain
{
    public class SenseNeuron : INeuron
    {
        private readonly ISense _sense;
        private readonly IEnumerable<ISynapse> _outputSynapses;

        private readonly float _rangeFactor;
        private readonly float _inputMedian;

        public IReadOnlyList<ISynapse> Outputs { get; }

        public SenseNeuron(ISense sense, IEnumerable<ISynapse> outputSynapses)
        {
            _sense = sense;
            _outputSynapses = outputSynapses;

            const float outputRange = 2; // -1 to +1
            var inputRange = sense.MaximumSensoryInput - sense.MinimumSensoryInput;
            _rangeFactor = outputRange / inputRange;
            _inputMedian = sense.MinimumSensoryInput + inputRange / 2f;
        }

        #region Implementation of INeuron

        /// <inheritdoc />
        public void FeedForward()
        {
            foreach (var outputSynapse in _outputSynapses)
            {
                outputSynapse.Input = Normalize(_sense.GetSensoryInput());
            }
        }

        #endregion

        private float Normalize(float input) => (input - _inputMedian) * _rangeFactor;
    }
}
