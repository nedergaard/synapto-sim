using FluentAssertions;
using Moq;
using SynaptoSimTests.Customizations;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class CreaturePresenterTests
    {
        [Theory]
        [AutoDomain]
        public void Tick_gives_brain_input_scaled_to_range_minus_one_to_positive_one(
            CreaturePresenterFixture fixture)
        {
            // Arrange
            var dut =
                fixture
                    .ClearSenses()
                    .WithSense(sensoryInput: 5f, minimumSensoryInput: -10f, maximumSensoryInput: 10f)
                    .WithSense(sensoryInput: -5f, minimumSensoryInput: -5f)
                    .WithSense(sensoryInput: 0f, maximumSensoryInput: 0f)
                    .NewDut();

            var expected = new[] { 0.5f, -1f, 1f };

            // Act
            dut.Tick();

            // Assert
            fixture.LastBrainFeed
                .Should()
                .BeEquivalentTo(expected);
        }


        [Theory]
        [AutoDomain]
        public void Tick_updates_capabilities_from_brain_after_providing_the_brain_inputs(
            float[] brainOutput, CreaturePresenterFixture fixture)
        {
            // Arrange
            // Wait to setup GetOutput until the brain has received input, to check that things are done in the right order.
            fixture.Brain
                .AsMock()
                .Setup(mock => mock.Feed(It.IsAny<float[]>()))
                .Callback(() =>
                    fixture.Brain
                        .AsMock()
                        .Setup(mock => mock.GetOutput())
                        .Returns(brainOutput));

            var dut = fixture.NewDut();

            // Act
            dut.Tick();

            // Assert
            fixture.Capabilities
                .Select(capability => capability.Excertion)
                .Should()
                .BeEquivalentTo(brainOutput);
        }
    }
}