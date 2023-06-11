using Assets.Scripts.Creature.Brain;
using FluentAssertions;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class SynapseTests
    {
        private const float Precision = 0.0005f;

        [Theory]
        [AutoDomainInlineData(0f, 1f, 0f)]
        [AutoDomainInlineData(0.5f, 0.7f, 0.35f)]
        [AutoDomainInlineData(-1f, 0.7f, -0.7f)]
        [AutoDomainInlineData(-0.1f, -0.01f, 0.001f)]
        public void WeightedOutput_returns_the_product_of_weight_and_input(float weight, float input, float expected)
        {
            // Arrange
            var dut = new Synapse(weight);

            // Act
            dut.Input = input;

            // Assert
            dut.WeightedOutput.Should().BeApproximately(expected, Precision);
        }
    }
}
