using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;

namespace VengineX.Graphics.Rendering.Textures
{
    // TODO implement smart texture binding to reduce gl calls and check if faster (bind texture only when needed)
    /// <summary>
    /// Base class for more specific textures
    /// </summary>
    public abstract class Texture : IDisposable, IBindable
    {
        /// <summary>
        /// Handle for the texture in VRAM.
        /// </summary>
        public uint Handle { get => _handle; }
        protected uint _handle;

        /// <summary>
        /// TextureTarget of this texture.
        /// </summary>
        public TextureTarget TextureTarget { get; }


        /// <summary>
        /// Generates a new texture with open gl as given texture target.
        /// </summary>
        /// <param name="target"></param>
        public Texture(TextureTarget target)
        {
            TextureTarget = target;
            GL.CreateTextures(target, 1, out _handle);
        }


        /// <summary>
        /// Overload for <see cref="Bind(TextureUnit)"/>.<br/>
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
        /// Use <see cref="Bind(TextureUnit)"/> if you want to bind to another texture unit.
        /// </summary>
        public void Bind() => Bind(0);


        /// <summary>
        /// Unbinds this texture.
        /// </summary>
        public void Unbind()
        {
            GL.ActiveTexture(0);
        }

        #endregion


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                GL.DeleteTexture(Handle);
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Texture()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
