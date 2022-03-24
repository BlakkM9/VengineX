namespace VengineX.ECS
{
    public abstract class Screen : IScreen
    {
        public abstract void Load();
        public abstract void Render(double delta);
        public abstract void Resize(int width, int height);
        public abstract void Unload();
        public abstract void Update(double delta);
    }
}
