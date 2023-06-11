using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;
using Unity.VisualScripting;

namespace Assets.Scripts.Creature.Brain
{
    public class BrainFactory
    {
        private record NeuronArguments
        {
            public List<ISynapse> Inputs { get; } = new();
            public List<ISynapse> Outputs { get; } = new();
        }

        private record SenseNeuronArguments(ISense Sense) : NeuronArguments
        {
            public ISense Sense { get; } = Sense;
        }

        private record InternalNeuronArguments(byte Id, float Bias) : NeuronArguments
        {
            public byte Id { get; } = Id;
            public float Bias { get; } = Bias;
        }

        private record CapabilityNeuronArguments(ICapability Capability) : NeuronArguments
        {
            public ICapability Capability { get; } = Capability;
        }

        private readonly Func<ISense, IEnumerable<ISynapse>, INeuron> _senseNeuronFactoryFunc;
        private readonly Func<ICapability, IEnumerable<ISynapse>, INeuron> _capabilityFactoryFunc;

        // TODO : Make factory method arguments nullable and use own implementation in that case
        public BrainFactory(
            Func<ISense, IEnumerable<ISynapse>, INeuron> senseNeuronFactoryFunc, 
            Func<ICapability, IEnumerable<ISynapse>, INeuron> capabilityFactoryFunc)
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

            var internalNeuronArguments =
                chromosomes
                    .Select(chromosome => NewNeuronArguments(chromosome[..neuronSequenceStringLength]))
                    .ToList();

            var senseNeuronArguments =
                senses
                    .Select(sense => new SenseNeuronArguments(sense))
                    .ToList();

            var capabilityNeuronArguments =
                capabilities
                    .Select(capability => new CapabilityNeuronArguments(capability))
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

                var synapse = new Synapse { Weight = weight };

                var targetNeuronInputs =
                    isInputNeuronASense
                        ? senseNeuronArguments[inputNeuronIndex].Outputs
                        : internalNeuronArguments[inputNeuronIndex].Outputs;
                targetNeuronInputs.Add(synapse);

                var targetNeuronOutputs =
                    isOutputNeuronACapability
                        ? capabilityNeuronArguments[outputNeuronIndex].Inputs
                        : internalNeuronArguments[outputNeuronIndex].Inputs;
                targetNeuronOutputs.Add(synapse);
            }

            result.Neurons
                .AddRange(senseNeuronArguments
                    .Select(args => _senseNeuronFactoryFunc(args.Sense, args.Outputs)));
            
            result.Neurons.AddRange(internalNeuronArguments.Select(NewNeuron));

            result.Neurons
                .AddRange(capabilityNeuronArguments
                    .Select(args => _capabilityFactoryFunc(args.Capability, args.Inputs)));

            return result;
        }

        private INeuron NewNeuron(InternalNeuronArguments args) => new Neuron(args.Id, args.Bias, x => x, args.Inputs, args.Outputs);

        private InternalNeuronArguments NewNeuronArguments(string neuronSequence) =>
            new(Id: byte.Parse(neuronSequence[..2], NumberStyles.AllowHexSpecifier),
                Bias: GetTwosComplementFrom13BitHex(neuronSequence));

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
