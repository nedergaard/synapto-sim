using Assets.Scripts.Creature.Sense;
using AutoFixture;
using Moq;

namespace SynaptoSimTests.Creature
{
    public class SenseBuilder
    {
        private readonly Fixture _fixture;
        private float _minimumSensoryInput;
        private float _maximumSensoryInput;

        private float _getSensoryInputResult;

        public SenseBuilder(Fixture fixture)
        {
            _fixture = fixture;

            _minimumSensoryInput = -1f;
            _maximumSensoryInput = 1f;

            _getSensoryInputResult = 0.5f;
        }

        public SenseBuilder WithInputRange(float minimumValue, float maximumValue)
        {
            _minimumSensoryInput = minimumValue;
            _maximumSensoryInput = maximumValue;

            return this;
        }

        public SenseBuilder WithSensoryInput(float sensoryInput)
        {
            _getSensoryInputResult = sensoryInput;

            return this;
        }

        public ISense Build()
        {
            var result = _fixture.Create<Mock<ISense>>();

            result
                .Setup(sense => sense.MinimumSensoryInput)
                .Returns(_minimumSensoryInput);

            result
                .Setup(sense => sense.MaximumSensoryInput)
                .Returns(_maximumSensoryInput);

            result
                .Setup(sense => sense.GetSensoryInput())
                .Returns(_getSensoryInputResult);

            return result.Object;
        }
    }
}
