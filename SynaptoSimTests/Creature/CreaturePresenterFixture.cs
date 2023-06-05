using Assets.Scripts.Creature;
using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;
using AutoFixture;
using Moq;
using SynaptoSimTests.Mocking;

namespace SynaptoSimTests.Creature
{
    public class CreaturePresenterFixture
    {
        private readonly Fixture _autoFixture;

        public IBrain Brain { get; private set; }                             
        public IList<ICapability> Capabilities { get; private set; }
        public IList<ISense> Senses { get; private set; }

        public bool WasBrainFeedForwardCalled { get; private set; }

        public CreaturePresenterFixture(Fixture autoFixture)
        {
            _autoFixture = autoFixture;

            Brain = _autoFixture.Create<IBrain>();
            Brain
                .AsMock()
                .Setup(brain => brain.FeedForward())
                .Callback(() => WasBrainFeedForwardCalled = true);

            Capabilities = _autoFixture.Create<List<ICapability>>();
            Senses = _autoFixture.Create<List<ISense>>();
        }

        public CreaturePresenterFixture ClearSenses()
        {
            Senses.Clear();

            return this;
        }

        public CreaturePresenterFixture WithSense(float sensoryInput, float minimumSensoryInput = -1f, float maximumSensoryInput = 1f)
        {
            var mockSense = _autoFixture.Create<Mock<ISense>>();

            mockSense
                .Setup(sense => sense.MinimumSensoryInput)
                .Returns(minimumSensoryInput);

            mockSense
                .Setup(sense => sense.MaximumSensoryInput)
                .Returns(maximumSensoryInput);

            mockSense
                .Setup(sense => sense.GetSensoryInput())
                .Returns(sensoryInput);

            Senses.Add(mockSense.Object);

            return this;
        }

        public CreaturePresenter NewDut() => new(Senses.ToArray(), Capabilities.ToArray(), Brain);
    }
}