using Assets.Scripts.Creature.Brain;
using Assets.Scripts.Creature.Capability;
using AutoFixture;

namespace SynaptoSimTests.Creature
{
    public class CapabilityNeuronFixture
    {
        private readonly IFixture _fixture;

        private Func<float, float> _activationFuncFunction;
        private SynapseArrayBuilder _inputSynapsesArrayBuilder;
        private CapabilityBuilder _capabilityBuilder;

        public ICapability Capability { get; private set; }

        public CapabilityNeuronFixture(IFixture fixture)
        {
            _fixture = fixture;

            _capabilityBuilder = _fixture.Create<CapabilityBuilder>();
            _capabilityBuilder.WithExertionRange(-1, 1);

            _inputSynapsesArrayBuilder = _fixture.Create<SynapseArrayBuilder>();
        }

        public CapabilityNeuronFixture With(CapabilityBuilder capabilityBuilder)
        {
            _capabilityBuilder = capabilityBuilder;

            return this;
        }

        public CapabilityNeuronFixture With(SynapseArrayBuilder inputSynapsesArrayBuilder)
        {
            _inputSynapsesArrayBuilder = inputSynapsesArrayBuilder;

            return this;
        }

        public CapabilityNeuronFixture WithActivationFunctionOutput(float output)
        {
            _activationFuncFunction = _ => output;

            return this;
        }

        public CapabilityNeuronFixture WithPassThroughActivationFunction()
        {
            _activationFuncFunction = input => input;

            return this;
        }

        public CapabilityNeuron Build()
        {
            Capability = _capabilityBuilder.Build();
            return new CapabilityNeuron(Capability, _inputSynapsesArrayBuilder.Build(), _activationFuncFunction);
        }
    }
}
