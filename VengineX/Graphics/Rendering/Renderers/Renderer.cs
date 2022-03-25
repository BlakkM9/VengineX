namespace VengineX.Graphics.Rendering.Batching
{

    public interface Renderer : IDisposable
    {
        public void Begin();
        public void End();
        public void Flush();
    }
}
