using Assets.Scripts.Creature.Brain;

namespace SynaptoSimTests.Creature
{
    public class NeuronFixture
    {
        private Func<float, float> _activationFuncFunction;
        private SynapseArrayBuilder _inputSynapsesArrayBuilder;
        private SynapseArrayBuilder _outputSynapsesArrayBuilder;

        private byte _id;
        private float _bias;

        public NeuronFixture(SynapseArrayBuilder inputSynapsesArrayBuilder, SynapseArrayBuilder outputSynapsesArrayBuilder)
        {
            _id = 0;
            _bias = 0;

            _inputSynapsesArrayBuilder = inputSynapsesArrayBuilder.WithSynapseCount(1);

            _outputSynapsesArrayBuilder = outputSynapsesArrayBuilder.WithSynapseCount(1);

            WithPassThroughActivationFunction();
        }

        public NeuronFixture WithInputsFrom(SynapseArrayBuilder inputSynapsesArrayBuilder)
        {
            _inputSynapsesArrayBuilder = inputSynapsesArrayBuilder;

            return this;
        }

        public NeuronFixture WithOutputsFrom(SynapseArrayBuilder outputSynapsesArrayBuilder)
        {
            _outputSynapsesArrayBuilder = outputSynapsesArrayBuilder;

            return this;
        }

        public NeuronFixture WithActivationFunctionOutput(float output)
        {
            _activationFuncFunction = _ => output;

            return this;
        }

        public NeuronFixture WithPassThroughActivationFunction()
        {
            _activationFuncFunction = input => input;
            return this;
        }

        public Neuron Build()
        {
            return 
                new Neuron(_id, _bias, 
                    _activationFuncFunction,
                    _inputSynapsesArrayBuilder.Build(), 
                    _outputSynapsesArrayBuilder.Build());
        }
    }
}
