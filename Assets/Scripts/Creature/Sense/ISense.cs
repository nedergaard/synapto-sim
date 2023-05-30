
namespace Assets.Scripts.Creature.Sense
{
    public interface ISense
    {
        /// <summary>
        /// The sensory input the sense is currently having from the world
        /// </summary>
        /// <returns>Unscaled sensory value</returns>
        float GetSensoryInput();

        float MinimumSensoryInput { get; }
        float MaximumSensoryInput { get; }
    }
}
