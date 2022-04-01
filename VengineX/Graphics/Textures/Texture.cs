using OpenTK.Graphics.OpenGL4;

namespace VengineX.Graphics.Rendering.Textures
{
    /// <summary>
    /// Base class for more specific textures
    /// </summary>
    public abstract class Texture : IDisposable, IBindable
    {
        /// <summary>
        /// Reference to the Handle for the texture in VRAM.
        /// </summary>
        public ref uint Handle { get => ref _handle; }

        /// <summary>
        /// Handle for the texture in VRAM.
        /// </summary>
        protected uint _handle;

        /// <summary>
        /// TextureTarget of this texture.
        /// </summary>
        public TextureTarget TextureTarget { get; }

        /// <summary>
        /// Generates a new texture with open gl as given texture target.
        /// </summary>
        public Texture(TextureTarget target)
        {
            TextureTarget = target;
            GL.CreateTextures(TextureTarget, 1, out Handle);
        }


        /// <summary>
        /// Binds this texture to given texture unit.
        /// <paramref name="unit"/> needs to be between 0 and 31.
        /// </summary>
        /// <param name="unit">Unit to bind to.</param>
        public void Bind(uint unit)
        {
            GL.BindTextureUnit(unit, Handle);
        }


        #region IBindable

        /// <summary>
        /// Binds to texture unit 0. Not to be used, only because implementing IBindable.<br/>
        /// Use <see cref="Bind(uint)"/> if you want to bind to another texture unit.
        /// </summary>
        public void Bind() => Bind(0);


        /// <summary>
        /// Unbinds this texture.
        /// </summary>
        public void Unbind()
        {
            GL.BindTextureUnit(0, 0);
        }

        #endregion


        #region IDisposable

        private bool _disposedValue;

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                GL.DeleteTexture(Handle);
                // Set large fields to null
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Texture()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
