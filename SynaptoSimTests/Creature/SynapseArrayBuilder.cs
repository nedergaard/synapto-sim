﻿using Assets.Scripts.Creature.Brain;
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

    public ISynapse[] Build()
    {
        var result =
            _fixture
                .CreateMany<ISynapse>(_weightedOutputs.Count)
                .ToArray();

        for (var index = 0; index < _weightedOutputs.Count; index++)
        {
            result[index]
                .AsMock()
                .Setup(mock => mock.WeightedOutput)
                .Returns(_weightedOutputs[index]);
        }

        return result;
    }
}