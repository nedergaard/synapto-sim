using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;
using FluentAssertions;
using SynaptoSimTests.Customizations;

namespace SynaptoSimTests.Creature;

public class BrainFactoryTests
{
    // "Neuron" in this context means "hidden" (internal) neuron.

    // Synapse gene:
    // Bits Meaning
    // ---- -------
    //    8 Id of input node
    //    8 Id of output node
    //    1 Input is from: 0 = sense node; 1 = internal node.
    //    1 Output is to: 0: internal node; 1 = capability node.
    //    1 <reserved> not used
    //   13 Connection strength. Converted to a value between -4.096 and 4.096. (2's complement / 1000 dec)

    // Neuron gene: 
    // Bits Meaning
    // ---- -------
    //    3 Function, e.g. ReLU
    //   13 Bias. Converted to a value between -4.096 and 4.096. (2's complement)

    // Chromosome:
    // is not really a thing is this model... Used to mean a section of the genome code containing 1 neuron and synapses.

    // Genome:
    // Bits Meaning
    // ---- -------
    //    8 Number of "extra" synapses per neuron in the genome. 0 = 2 synapses/neuron in the code. Does not mean that a neuron cannot have more synapses connected that this. It is just a way to read the genome.
    // Rest (16 Neuron bits, <above value> * 32 Synapse bit) repeated

    // Note: not all brains can be encoded back to the original genome. Some synapses and neurons may be optimized out,
    // and ids of neuron cannot be preserved in all cases. E.g. when there are fewer senses, neurons, and/or capabilities than can be addressed
    // then the encoded id may be mapped onto the available neurons.

    [Theory]
    [AutoDomain]
    public void BuildFromGenome_returns_expected_brain(
        BrainFactoryFixture fixture, ISense[] senses, ICapability[] capabilities)
    {
        // Arrange

        /*  3 (2) senses           1 neuron         2 capabilities
         *    S -----  w: 2.047
         *           \ 
         *    S       >=========== [ neuron ] -----W:1.804-- C     
         *           / w:-2.047    bias: 0.2 
         *    S ----<                
         *           \-----w:4.095 ------------------------- C
         */

        var genome =
            "02" + // 2 additional synapses per hidden neuron
            /* neuron */
            "00C8" + // Function 0, bias 0.2
            /* synapse 1 */
            "00" + // Input from neuron id 0
            "00" + // Output to neuron id 0
            "07FF" + // Input from sense neuron (bit=0), output to internal neuron (bit=0), <not used> bit=0, Weight 2.047 (bits=0_0111_1111_1111)
            /* synapse 2 */
            "02" + // Input from neuron id 2
            "00" + // Output to neuron id 0
            "1801" + // Input from sense neuron (bit=0), output to internal neuron (bit=0), <not used> bit=0, Weight -2.047 (bits=1_1000_0000_0001)
            /* synapse 3 */
            "02" + // Input from neuron id 2
            "01" + // Output to neuron id 1
            "4555" + // Input from sense neuron (bit=0), output to capability neuron (bit=1), <not used> bit=0, Weight 1.365 (bits=0_0101_0101_0101)
            /* synapse 4 */
            "00" + // Input from neuron id 0
            "00" + // Output to neuron id 0
            "CFFF"; // Input from internal neuron (bit=1), output to capability neuron (bit=1), <not used> bit=0, Weight 1.804 (bits=0_1111_1111_1111)

        var neuron =
            new
            {
                Id = 0,
                Bias = 0.2f,
            };

        var senseNeuron0 = new { Sense = senses[0] };
        var senseNeuron2 = new { Sense = senses[2] };

        var capabilityNeuron0 = new { Capability = capabilities[0] };
        var capabilityNeuron1 = new { Capability = capabilities[1] }; 

        var expected =
            new
            {
                Synapses = 
                    new object[]
                    {
                        new
                        {
                            InputNeuron = senseNeuron0,
                            OutputNeuron = neuron,
                            Weight = 2.047f,
                        },
                        new
                        {
                            InputNeuron = senseNeuron2,
                            OutputNeuron = neuron,
                            Weight = -2.047f,
                        },
                        new
                        {
                            InputNeuron = senseNeuron2,
                            OutputNeuron = capabilityNeuron1,
                            Weight = 1.365f,
                        },
                        new
                        {
                            InputNeuron = neuron,
                            OutputNeuron = capabilityNeuron0,
                            Weight = 4.095f,
                        },
                    },
            };

        var dut = fixture.NewDut();

        // Act
        var actual = dut.BuildFrom(genome, senses, capabilities);

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }

    [Theory]
    [AutoDomain]
    public void BuildFromGenome_sets_correct_bias_on_internal_neurons(
        BrainFactoryFixture fixture, ISense[] senses, ICapability[] capabilities)
    {
        // Arrange

        /*   1 sense           2 neurons         1 capability
         *            -------- [ neuron ] -----
         *           /         bias: 4.0       \
         *    S ----<                           >==== C
         *           \                         /
         *            -------- [ neuron ] -----
         *                     bias: -3.876 
         */

        // Some of the synapses are there just to ensure nothing gets optimized out.

        var genome =
            "00" + // 2:1 synapses:neuron
            /* neuron */
            "0FA0" + // Function 0, bias 4.0
            /* synapse 1 */
            "00" + // Input from neuron id 0
            "00" + // Output to neuron id 0
            "0000" + // Input from sense neuron (bit=0), output to internal neuron (bit=0), <not used> bit=0, Weight 0
            /* synapse 2 */
            "00" + // Input from neuron id 0
            "01" + // Output to neuron id 1
            "0000" + // Input from sense neuron (bit=0), output to internal neuron (bit=0), <not used> bit=0, Weight 0
            /* neuron */
            "10DC" + // Function 0, bias -3.876
            /* synapse 3 */
            "00" + // Input from neuron id 0
            "00" + // Output to neuron id 1
            "C000" + // Input from internal neuron (bit=1), output to capability neuron (bit=1), <not used> bit=0, Weight 0
            /* synapse 4 */
            "01" + // Input from neuron id 1
            "00" + // Output to neuron id 0
            "C000"; // Input from internal neuron (bit=1), output to capability neuron (bit=1), <not used> bit=0, Weight 0

        var neuron0 = new { Bias = 4.0f };
        var neuron1 = new { Bias = -3.876f };

        var expected =
            new
            {
                Synapses = 
                    new object[]
                    {
                        new { OutputNeuron = neuron0 },
                        new { OutputNeuron = neuron1 },
                        new { InputNeuron = neuron0 },
                        new { InputNeuron = neuron1 },
                    },
            };

        var dut = fixture.NewDut();

        // Act
        var actual = dut.BuildFrom(genome, senses, capabilities);

        // Assert
        actual.Should()
            .BeEquivalentTo(expected);
    }
}