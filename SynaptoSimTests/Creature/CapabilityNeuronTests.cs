using AutoFixture.Xunit2;
using FluentAssertions;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class CapabilityNeuronTests
    {
        private const float Precision = 0.001f;

        [Theory]
        [InlineAutoData(0, 10, 20, 15)]
        [InlineAutoData(0.5, -5, 15, 10)]
        [InlineAutoData(-1 , - 10, 10, -10)]
        [InlineAutoData(0, -20, 0, -10)]
        [InlineAutoData(0, -40, -20, -30)]
        [InlineAutoData(0.7, 80, 100, 97)]
        [InlineAutoData(1, 3, 7, 7)]
        public void FeedForward_normalizes_output_to_capability_exertion(
            float synapseOutput, float minimumExertion, float maximumExertion, float expected,
            CapabilityBuilder capabilityBuilder, CapabilityNeuronFixture fixture)
        {
            // Arrange
            var dut = 
                fixture
                    .With(capabilityBuilder
                        .WithExertionRange(minimumExertion, maximumExertion))
                    .WithActivationFunctionOutput(synapseOutput)
                .Build();
            
            // Act
            dut.FeedForward();

            // Assert
            fixture.Capability.Exertion.Should().Be(expected);
        }

        [Theory]
        [AutoDomainInlineData(0.1f, 0.3f, 0.4f)]
        [AutoDomainInlineData(0.5f, -0.35f, 0.15f)]
        public void FeedForward_sums_weightedoutput_from_input_synapses(
            float output1, float output2, float expected, 
            SynapseArrayBuilder synapseArrayBuilder, CapabilityNeuronFixture fixture)
        {
            // Arrange
            var dut =
                fixture
                    .With(synapseArrayBuilder
                        .WithSynapse(weightedOutput: output1)
                        .WithSynapse(weightedOutput: output2))
                    .WithPassThroughActivationFunction()
                .Build();

            // Act
            dut.FeedForward();

            // Assert
            fixture.Capability.Exertion.Should().BeApproximately(expected, Precision);
        }
    }
}