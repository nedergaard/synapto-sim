using Assets.Scripts.Creature.Brain;
using AutoFixture;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature;

public class SynapseArrayBuilder
{
    private readonly List<float> _weightedOutputs;
    private readonly IFixture _fixture;

    public SynapseArrayBuilder(IFixture fixture)
    {
        _fixture = fixture;
        _weightedOutputs = new List<float>();
    }

    public SynapseArrayBuilder WithSynapse(float weightedOutput)
    {
        _weightedOutputs.Add(weightedOutput);

        return this;
    }
    public SynapseArrayBuilder WithSynapseCount(int synapseCount)
    {
        _weightedOutputs.AddRange(_fixture.CreateMany<float>(synapseCount));

        return this;
    }

    public ISynapse[] Build()
    {
        var result =
            _fixture
                .CreateMany<ISynapse>(_weightedOutputs.Count)
                .ToArray();

        for (var index = 0; index < _weightedOutputs.Count; index++)
        {
            var mock = result[index].AsMock();

            mock
                .SetupAllProperties()
                .Setup(mock => mock.WeightedOutput)
                .Returns(_weightedOutputs[index]);
        }

        return result;
    }
}