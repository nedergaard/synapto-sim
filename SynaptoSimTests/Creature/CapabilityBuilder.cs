using Assets.Scripts.Creature.Capability;
using AutoFixture;
using Moq;

namespace SynaptoSimTests.Creature;

public class CapabilityBuilder
{
    private readonly Fixture _fixture;
    private float _minimumExertion;
    private float _maximumExertion;

    public CapabilityBuilder(Fixture fixture)
    {
        _fixture = fixture;

        _minimumExertion = 0;
        _maximumExertion = 10;
    }

    public CapabilityBuilder WithExertionRange(float minimumExertion, float maximumExertion)
    {
        _minimumExertion = minimumExertion;
        _maximumExertion = maximumExertion;

        return this;
    }

    public ICapability Build()
    {
        var result = _fixture.Create<Mock<ICapability>>();

        result
            .Setup(mock => mock.MinimumExertion)
            .Returns(_minimumExertion);

        result
            .Setup(mock => mock.MaximumExertion)
            .Returns(_maximumExertion);

        result.SetupProperty(mock => mock.Exertion);

        return result.Object;
    }
}