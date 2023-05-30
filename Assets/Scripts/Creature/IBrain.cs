namespace Assets.Scripts.Creature
{
    public interface IBrain
    {
        public void Feed(float[] inputs);

        public float[] GetOutput();
    }
}
