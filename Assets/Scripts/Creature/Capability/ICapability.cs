namespace Assets.Scripts.Creature.Capability
{
    public interface ICapability
    {
        float Exertion { get; set; }

        float MinimumExertion { get; }
        float MaximumExertion { get; }
    }
}
