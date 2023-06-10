using System;
using System.Collections.Generic;
using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;

namespace Assets.Scripts.Creature.Brain
{
    public class Brain : IBrain
    {
        public Brain()
        {
            Neurons = new List<INeuron>();
            Senses = new List<ISense>();
            Capabilities = new List<ICapability>();
        }

        #region Implementation of IBrain

        public IList<INeuron> Neurons{ get; }

        public IList<ISense> Senses { get; }

        public IList<ICapability> Capabilities { get; }

        /// <inheritdoc />
        public void FeedForward()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public float[] GetOutput()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}