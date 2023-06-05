namespace Assets.Scripts.Creature
{
    public interface IBrain
    {
        float[] GetOutput();
        
        void FeedForward();
    }
}
