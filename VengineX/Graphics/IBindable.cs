namespace VengineX.Graphics
{
    /// <summary>
    /// Interface for bindable OpenGL resources.
    /// </summary>
    public interface IBindable
    {
        /// <summary>
        /// Binds the resource to the current render state.
        /// </summary>
        void Bind();


        /// <summary>
        /// Unbinds the resource from the current render state.
        /// </summary>
        void Unbind();
    }
}
