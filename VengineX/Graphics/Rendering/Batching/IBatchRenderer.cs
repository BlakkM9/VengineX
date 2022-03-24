namespace VengineX.Graphics.Rendering.Batching
{

    public interface IBatchRenderer : IDisposable
    {
        public void Begin();
        public void End();
        public void Flush();
    }
}
