using Assets.Scripts.Creature.Capability;
using Assets.Scripts.Creature.Sense;

namespace Assets.Scripts.Creature
{
    public class CreaturePresenter
    {
        private readonly IBrain _brain;
        private readonly ICapability[] _capabilities;
        private readonly ISense[] _senses;

        public CreaturePresenter(ISense[] senses, ICapability[] capabilities, IBrain brain)
        {
            _senses = senses;
            _capabilities = capabilities;
            _brain = brain;
        }

        public void Tick()
        {
            _brain.FeedForward();

            var brainOutput = _brain.GetOutput();
            for (var i = 0; i < brainOutput.Length; i++)
            {
                _capabilities[i].Excertion = brainOutput[i];
            }
        }
    }
}
