using System;
using Assets.Scripts.Creature.Capability;

namespace Assets.Scripts.Creature.Brain
{
    public class CapabilityNeuron
    {
        private readonly ICapability _capability;
        private readonly Func<float> _activationFunctionFunc;
        
        private readonly float _outputRange;
        private const float InputRange = 2f;  // -1 to +1

        public CapabilityNeuron(ICapability capability, Func<float> activationFunctionFunc)
        {
            _capability = capability;
            _activationFunctionFunc = activationFunctionFunc;

            _outputRange = _capability.MaximumExertion - capability.MinimumExertion;
        }

        public void FeedForward()
        {
            _capability.Exertion = Normalize(_activationFunctionFunc());
        }

        private float Normalize(float input)
        {
            return (input - -1f) / InputRange * _outputRange + _capability.MinimumExertion;
        }
    }
}
