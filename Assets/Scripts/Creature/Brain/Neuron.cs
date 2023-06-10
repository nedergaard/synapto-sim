using System.Collections.Generic;
using Unity.VisualScripting;

namespace Assets.Scripts.Creature.Brain
{
    public class Neuron : INeuron
    {
        private readonly IEnumerable<ISynapse> _inputs;
        private readonly IEnumerable<ISynapse> _outputs;
        
        public byte Id { get; }
        public float Bias { get; }

        public IReadOnlyList<ISynapse> Inputs => _inputs.AsReadOnlyList() as IReadOnlyList<ISynapse>;

        public IReadOnlyList<ISynapse> Outputs => _outputs.AsReadOnlyList() as IReadOnlyList<ISynapse>;

        public Neuron(byte id, float bias, IEnumerable<ISynapse> inputs, IEnumerable<ISynapse> outputs)
        {
            Id = id;
            Bias = bias;
            _inputs = inputs;
            _outputs = outputs;
        }

        #region Implementation of INeuron

        /// <inheritdoc />
        public void FeedForward()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}