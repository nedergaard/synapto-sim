using Assets.Scripts.Creature.Brain;
using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;

namespace SynaptoSimTests.Creature;

public class BrainFactoryFixture
{
    public BrainFactory NewDut()
    {
        return new BrainFactory(NewSenseNeuronFunc, NewCapabilityNeuronFunc);
    }

    private INeuron NewSenseNeuronFunc(ISense sense) => new MockSenseNeuron { Sense = sense };
    private INeuron NewCapabilityNeuronFunc(ICapability capability) => new MockCapabilityNeuron { Capability = capability };
}

public class MockSenseNeuron : INeuron
{
    public ISense Sense { get; set; }
}

public class MockCapabilityNeuron : INeuron
{
    public ICapability Capability { get; set; }
}