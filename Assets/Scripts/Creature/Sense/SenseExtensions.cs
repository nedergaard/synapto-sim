
namespace Assets.Scripts.Creature.Sense
{
    public static class SenseExtensions
    {
        public static float GetScaledSensoryInput(this ISense sense)
        {
            var range = sense.MaximumSensoryInput - sense.MinimumSensoryInput;

            var rawInput = sense.GetSensoryInput();

            var positiveInput = rawInput - sense.MinimumSensoryInput;

            var result = 2 * positiveInput / range - 1f;
            
            return result;
        }
    }
}
