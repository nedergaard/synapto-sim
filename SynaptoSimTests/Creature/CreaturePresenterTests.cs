using FluentAssertions;
using SynaptoSimTests.Customizations;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class CreaturePresenterTests
    {
        [Theory]
        [AutoDomain]
        public void Tick_gives_brain_signal_to_feedforward(
            CreaturePresenterFixture fixture)
        {
            // Arrange
            var dut = fixture.NewDut();

            // Act
            dut.Tick();

            // Assert
            fixture.WasBrainFeedForwardCalled.Should().Be(true);
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
                .Setup(mock => mock.FeedForward())
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