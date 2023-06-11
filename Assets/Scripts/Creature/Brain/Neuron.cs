using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Assets.Scripts.Creature.Brain
{
    public class Neuron : INeuron
    {
        private readonly Func<float, float> _activationFunctionFunc;
        private readonly IEnumerable<ISynapse> _inputs;
        private readonly IEnumerable<ISynapse> _outputs;
        
        public byte Id { get; }
        public float Bias { get; }

        public IReadOnlyList<ISynapse> Inputs => _inputs.AsReadOnlyList() as IReadOnlyList<ISynapse>;

        public IReadOnlyList<ISynapse> Outputs => _outputs.AsReadOnlyList() as IReadOnlyList<ISynapse>;

        public Neuron(byte id, float bias, Func<float, float> activationFunctionFunc, IEnumerable<ISynapse> inputs, IEnumerable<ISynapse> outputs)
        {
            Id = id;
            Bias = bias;
            _activationFunctionFunc = activationFunctionFunc;
            _inputs = inputs;
            _outputs = outputs;
        }

        #region Implementation of INeuron

        /// <inheritdoc />
        public void FeedForward()
        {
            var weightedSum = _inputs.Sum(input => input.WeightedOutput);
            var functionOutput = _activationFunctionFunc(weightedSum);
            foreach (var output in _outputs)
            {
                output.Input = functionOutput;
            }
        }

        #endregion
    }
}