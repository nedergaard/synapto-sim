using Assets.Scripts.Creature.Brain;
using FluentAssertions;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class SenseNeuronTests
    {
        [Theory]
        [AutoDomainInlineData(0, 20, 3, -0.7)]
        [AutoDomainInlineData(-5, 15, 10, 0.5)]
        [AutoDomainInlineData(-10, 10, -10, -1)]
        [AutoDomainInlineData(-20, 0, -10, 0)]
        [AutoDomainInlineData(-40, -20, -30, 0)]
        [AutoDomainInlineData(80, 100, 97, 0.7)]
        [AutoDomainInlineData(3, 7, 7, 1)]
        public void FeedForward_normalizes_sense_value(
            float minimumInput, float maximumInput, float currentInput, float expected,
            SenseBuilder senseBuilder, ISynapse outputSynapse)
        {
            // Arrange
            var sense =
                senseBuilder
                    .WithInputRange(minimumInput, maximumInput)
                    .WithSensoryInput(currentInput)
                    .Build();

            var dut = new SenseNeuron(sense, new[] { outputSynapse });

            // Act
            dut.FeedForward();

            // Assert
            outputSynapse.Input.Should().Be(expected);
        }
    }
}
