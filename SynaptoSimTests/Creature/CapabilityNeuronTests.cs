using Assets.Scripts.Creature.Brain;
using AutoFixture.Xunit2;
using FluentAssertions;

namespace SynaptoSimTests.Creature
{
    public class CapabilityNeuronTests
    {
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
            CapabilityBuilder capabilityBuilder)
        {
            // Arrange
            var capability =
                capabilityBuilder
                    .WithExertionRange(minimumExertion, maximumExertion)
                    .Build();

            var dut = new CapabilityNeuron(capability, () => synapseOutput);

            // Act
            dut.FeedForward();

            // Assert
            capability.Exertion.Should()
                .Be(expected);
        }
    }
}