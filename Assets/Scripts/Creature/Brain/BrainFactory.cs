using System;
using System.Globalization;
using System.Linq;
using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;

namespace Assets.Scripts.Creature.Brain
{
    public class BrainFactory
    {
        private readonly Func<ISense, INeuron> _senseNeuronFactoryFunc;
        private readonly Func<ICapability, INeuron> _capabilityFactoryFunc;

        public BrainFactory(Func<ISense, INeuron> senseNeuronFactoryFunc, Func<ICapability, INeuron> capabilityFactoryFunc)
        {
            _senseNeuronFactoryFunc = senseNeuronFactoryFunc;
            _capabilityFactoryFunc = capabilityFactoryFunc;
        }

        public IBrain BuildFrom(string genome, ISense[] senses, ICapability[] capabilities)
        {
            const int baseNumberOfSynapsesPerNeuron = 2;
            const int neuronSequenceStringLength = 4;
            const int synapseSequenceStringLength = 8;

            var result = new Brain();

            var numberOfSynapsesPerNeuron =
                int.Parse(
                    genome[..2])
                + baseNumberOfSynapsesPerNeuron;

            var stride = neuronSequenceStringLength + numberOfSynapsesPerNeuron * synapseSequenceStringLength;

            var chromosomes =
                genome[2..]
                    .Chunks(stride)
                    .ToList();

            var internalNeurons =
                chromosomes
                    .Select(chromosome => chromosome[..neuronSequenceStringLength])
                    .Select(GetNeuron)
                    .ToList();

            var senseNeurons =
                senses
                    .Select(_senseNeuronFactoryFunc)
                    .ToList();

            var capabilityNeurons =
                capabilities
                    .Select(_capabilityFactoryFunc)
                    .ToList();

            foreach (var synapseSequence in
                     chromosomes
                         .SelectMany(chromosome => chromosome[neuronSequenceStringLength..])
                         .Chunks(synapseSequenceStringLength))
            {
                var inputNeuronIndex = int.Parse(synapseSequence[..2].AsSpan(), NumberStyles.AllowHexSpecifier);
                var outputNeuronIndex = int.Parse(synapseSequence[2..4].AsSpan(), NumberStyles.AllowHexSpecifier);

                var neuronTypesCode = uint.Parse(synapseSequence[4..].AsSpan(), NumberStyles.AllowHexSpecifier);
                var isInputNeuronASense = (neuronTypesCode & 0x8000) == 0;
                var isOutputNeuronACapability = (neuronTypesCode & 0x4000) > 0;

                var weight = GetTwosComplementFrom13BitHex(synapseSequence[4..]);

                result.Synapses
                    .Add(
                        new Synapse
                        {
                            InputNeuron =
                                isInputNeuronASense
                                    ? senseNeurons[inputNeuronIndex]
                                    : internalNeurons[inputNeuronIndex],
                            OutputNeuron =
                                isOutputNeuronACapability
                                    ? capabilityNeurons[outputNeuronIndex]
                                    : internalNeurons[outputNeuronIndex],
                            Weight = weight,
                        });
            }

            return result;
        }

        private INeuron GetNeuron(string neuronSequence)
        {
            return
                new Neuron
                {
                    Id = byte.Parse(neuronSequence[..2], NumberStyles.AllowHexSpecifier),
                    Bias = GetTwosComplementFrom13BitHex(neuronSequence),
                };
        }

        private float GetTwosComplementFrom13BitHex(ReadOnlySpan<char> hex)
        {
            var result = short.Parse(hex, NumberStyles.AllowHexSpecifier) & 0x1FFF;
            // Is MSB set then it is a negative value
            if ((result & 0x1000) > 0)
            {
                result -= 0x2000;
            }
            return result / 1000f;
        }
    }
}
