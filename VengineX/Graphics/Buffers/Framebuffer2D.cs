using OpenTK.Graphics.OpenGL4;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Textures;

namespace VengineX.Graphics.Buffers
{
    /// <summary>
    /// Class representing a framebuffer with a 2D output texture.<br/>
    /// There is also a renderbuffer attached with <see cref="RenderbufferStorage.Depth24Stencil8"/>.
    /// </summary>
    public class Framebuffer2D : Framebuffer<Texture2D>, IBindable, IDisposable
    {

        protected uint _rbo;

        /// <summary>
        /// Creates a new framebuffer with a 2D output texture <see cref="FramebufferAttachment.ColorAttachment0"/>.
        /// </summary>
        /// <param name="width">Width of the framebuffers output texture.</param>
        /// <param name="height">Height of the framebuffers output texture.</param>
        /// <param name="internalFormat">Internal pixel format of the framebuffers output texture.</param>
        /// <param name="pixelFormat">Pixel format of the framebuffers output texture.</param>
        /// <param name="attachDepthAndStenchil">Wether or not to attach an Renderbuffer to save stencil and depth aswell.</param>
        public Framebuffer2D(
            int width, int height,
            SizedInternalFormat internalFormat,
            PixelFormat pixelFormat,
            bool attachDepthAndStenchil) : base()
        {
            GL.CreateFramebuffers(1, out _fbo);

            // Create output texture
            Texture2DParameters parameters = new Texture2DParameters()
            {
                Width = width,
                Height = height,
                InternalFormat = internalFormat,
                PixelFormat = pixelFormat,
                PixelType = PixelType.UnsignedByte,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapModeS = TextureWrapMode.Repeat,
                WrapModeT = TextureWrapMode.Repeat,
                GenerateMipmaps = false,
            };

            OutputTexture = new Texture2D(ref parameters);


            // Bind output texture to framebuffer
            GL.NamedFramebufferTexture(
                _fbo,
                FramebufferAttachment.ColorAttachment0,
                OutputTexture.Handle,
                0);



            if (attachDepthAndStenchil)
            {
                // Create and attach renderbuffer for depth and stencil.
                GL.CreateRenderbuffers(1, out _rbo);
                GL.NamedRenderbufferStorage(_fbo, RenderbufferStorage.Depth24Stencil8, width, height);
                GL.NamedFramebufferRenderbuffer(_fbo, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _rbo);
            }


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

        public void Clear(ClearBuffer clearBuffer, float[] value)
        {
            GL.ClearNamedFramebuffer(_fbo, clearBuffer, 0, value);
        }


        #region IBindable

        /// <summary>
        /// Binds this framebuffer to the current renderer state.
        /// </summary>
        public void Bind()
        {
            // Save viewport dimensions before bind
            GL.GetInteger(GetPName.Viewport, _viewportBeforeBind);

            GL.Viewport(0, 0, OutputTexture.Width, OutputTexture.Height);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        }


        /// <summary>
        /// Unbind this framebuffer from the current renderer state (binds default framebuffer again).
        /// </summary>
        public void Unbind()
        {
            GL.Viewport(_viewportBeforeBind[0], _viewportBeforeBind[1], _viewportBeforeBind[2], _viewportBeforeBind[3]);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
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
                    if (!IsTextureDetached) { OutputTexture.Dispose(); }
                }

                GL.DeleteFramebuffer(_fbo);
                if (_rbo != 0)
                {
                    GL.DeleteRenderbuffer(_rbo);
                }
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        ~Framebuffer2D()
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
