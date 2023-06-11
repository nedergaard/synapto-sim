using FluentAssertions;
using SynaptoSimTests.Customizations;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class NeuronTests
    {
        private const float Precision = 0.001f;
        private const int Many = 3;

        [Theory]
        [AutoDomainInlineData(0.2f, 0.55f, 0.75f)]
        [AutoDomainInlineData(-0.5f, 0.35f, -0.15f)]
        public void FeedForward_sums_weightedoutput_from_input_synapses(
            float output1, float output2, float expected,
            SynapseArrayBuilder synapseArrayBuilder, NeuronFixture fixture)
        {
            // Arrange
            var dut =
                fixture
                    .WithInputsFrom(
                        synapseArrayBuilder
                            .WithSynapse(weightedOutput: output1)
                            .WithSynapse(weightedOutput: output2))
                    .Build();

            // Act
            dut.FeedForward();

            // Assert
            dut.Outputs
                .First().Input
                .Should().BeApproximately(expected, Precision);
        }

        [Theory]
        [AutoDomain]
        public void FeedForward_distributes_activationfunction_output_to_all_output_synapses(
            float expected, 
            SynapseArrayBuilder synapseArrayBuilder, NeuronFixture fixture)
        {
            // Arrange
            var dut =
                fixture
                    .WithOutputsFrom(
                        synapseArrayBuilder.WithSynapseCount(Many))
                    .WithActivationFunctionOutput(expected)
                    .Build();

            // Act
            dut.FeedForward();

            // Assert
            dut.Outputs
                .Should()
                .AllSatisfy(item =>
                    item.Input
                        .Should()
                        .BeApproximately(expected, Precision));
        }

        [Theory]
        [AutoDomainInlineData(0.2f, 0.3f, -1.6f, -1.1f)]
        [AutoDomainInlineData(-0.5f, -0.1f, -0.2f, -0.8f)]
        public void FeedForward_applies_bias_to_sum_of_weightedoutput(
            float weightedOuput1, float weightedOuput2, float bias, float expected,
            SynapseArrayBuilder synapseArrayBuilder, NeuronFixture fixture)
        {
            // Arrange
            var dut =
                fixture
                    .WithBias(bias)
                    .WithInputsFrom(
                        synapseArrayBuilder
                            .WithSynapse(weightedOutput: weightedOuput1)
                            .WithSynapse(weightedOutput: weightedOuput2))
                    .Build();

            // Act
            dut.FeedForward();

            // Assert
            dut.Outputs
                .First().Input
                .Should().BeApproximately(expected, Precision);
        }

    }
}
