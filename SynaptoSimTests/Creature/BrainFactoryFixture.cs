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

    private INeuron NewSenseNeuronFunc(ISense sense, IEnumerable<ISynapse> outputs) => 
        new MockSenseNeuron
        {
            Sense = sense,
            Outputs = outputs.ToList(),
        };

    private INeuron NewCapabilityNeuronFunc(ICapability capability, IEnumerable<ISynapse> inputs) => 
        new MockCapabilityNeuron
        {
            Capability = capability,
            Inputs = inputs.ToList(),
        };
}

public class MockNeuron : INeuron
{
    #region Implementation of INeuron

    /// <inheritdoc />
    public void FeedForward()
    {
        throw new NotImplementedException();
    }

    #endregion
}

public class MockSenseNeuron : MockNeuron
{
    public ISense Sense { get; set; }

    public IEnumerable<ISynapse> Outputs { get; set; }
}

public class MockCapabilityNeuron : MockNeuron
{
    public ICapability Capability { get; set; }
    public IEnumerable<ISynapse> Inputs { get; set; }
}