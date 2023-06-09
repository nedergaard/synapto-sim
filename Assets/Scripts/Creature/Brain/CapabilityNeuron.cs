﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Creature.Capability;

namespace Assets.Scripts.Creature.Brain
{
    public class CapabilityNeuron : INeuron
    {
        private readonly ICapability _capability;
        private readonly ISynapse[] _inputSynapses;
        private readonly Func<float, float> _activationFunctionFunc;
        
        private readonly float _outputRange;
        private const float InputRange = 2f;  // -1 to +1

        public CapabilityNeuron(ICapability capability, ISynapse[] inputSynapses, Func<float, float> activationFunctionFunc)
        {
            _capability = capability;
            _inputSynapses = inputSynapses;
            _activationFunctionFunc = activationFunctionFunc;

            _outputRange = _capability.MaximumExertion - capability.MinimumExertion;
        }

        public IReadOnlyList<ISynapse> Outputs { get; }

        public void FeedForward()
        {
            _capability.Exertion =
                Normalize(
                    _activationFunctionFunc(_inputSynapses.Sum(synapse => synapse.WeightedOutput)));
        }

        private float Normalize(float input)
        {
            return (input - -1f) / InputRange * _outputRange + _capability.MinimumExertion;
        }
    }
}
