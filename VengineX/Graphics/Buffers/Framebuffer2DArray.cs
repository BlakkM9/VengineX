using OpenTK.Graphics.OpenGL4;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Textures;

namespace VengineX.Graphics.Buffers
{
    public class Framebuffer2DArray : Framebuffer<Texture2DArray>, IDisposable, IBindable
    {
        public Framebuffer2DArray(
            int width, int height, int layers,
            SizedInternalFormat internalFormat,
            PixelFormat pixelFormat)
        {
            GL.CreateFramebuffers(1, out _fbo);

            // Create output texture
            Texture2DArrayParameters parameters = new Texture2DArrayParameters()
            {
                Width = width,
                Height = height,
                Layers = layers,
                InternalFormat = internalFormat,
                PixelFormat = pixelFormat,
                PixelType = PixelType.UnsignedByte,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapModeS = TextureWrapMode.ClampToBorder,
                WrapModeT = TextureWrapMode.ClampToBorder,
                WrapModeR = TextureWrapMode.ClampToBorder,
                GenerateMipmaps = false,
            };

            OutputTexture = new Texture2DArray(ref parameters);
            OutputTexture.Bind();

            GL.NamedFramebufferTextureLayer(_fbo, FramebufferAttachment.ColorAttachment0, OutputTexture.Handle, 0, 0);


            // Check for completion
            FramebufferStatus status = GL.CheckNamedFramebufferStatus(_fbo, FramebufferTarget.Framebuffer);
            if (status != FramebufferStatus.FramebufferComplete)
            {
                Logger.Log(Severity.Error, "Failed to create framebuffer: " + status);
            }
            else
            {
                Logger.Log(Severity.Info, $"Created framebuffer, size: {width}x{height}");
            }


            // Unbind framebuffer.
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }


        public void Bind(int layer)
        {
            // Save viewport dimensions before bind
            GL.GetInteger(GetPName.Viewport, _viewportBeforeBind);

            GL.Viewport(0, 0, OutputTexture.Width, OutputTexture.Height);

            GL.NamedFramebufferTextureLayer(_fbo, FramebufferAttachment.ColorAttachment0, OutputTexture.Handle, 0, layer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        }


        public void Bind() => Bind(0);


        public void Unbind()
        {
            GL.Viewport(_viewportBeforeBind[0], _viewportBeforeBind[1], _viewportBeforeBind[2], _viewportBeforeBind[3]);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    if (!IsTextureDetached) { OutputTexture.Dispose(); }
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                GL.DeleteBuffer(_fbo);
                // Set large fields to null
                _disposedValue = true;
            }
        }

        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Framebuffer2DArray()
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
