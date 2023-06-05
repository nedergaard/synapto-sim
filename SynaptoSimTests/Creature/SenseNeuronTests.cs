using Assets.Scripts.Creature.Brain;
using AutoFixture.Xunit2;
using FluentAssertions;

namespace SynaptoSimTests.Creature
{
    public class SenseNeuronTests
    {
        [Theory]
        [InlineAutoData(0, 20, 3, -0.7)]
        [InlineAutoData(-5, 15, 10, 0.5)]
        [InlineAutoData(-10, 10, -10, -1)]
        [InlineAutoData(-20, 0, -10, 0)]
        [InlineAutoData(-40, -20, -30, 0)]
        [InlineAutoData(80, 100, 97, 0.7)]
        [InlineAutoData(3, 7, 7, 1)]
        public void FeedForward_normalizes_sense_value(
            float minimumInput, float maximumInput, float currentInput, float expected,
            SenseBuilder senseBuilder)
        {
            // Arrange
            var sense =
                senseBuilder
                    .WithInputRange(minimumInput, maximumInput)
                    .WithSensoryInput(currentInput)
                    .Build();

            var dut = new SenseNeuron(sense);

            // Act
            dut.FeedForward();

            // Assert
            dut.Output.Should().Be(expected);
        }
    }
}
