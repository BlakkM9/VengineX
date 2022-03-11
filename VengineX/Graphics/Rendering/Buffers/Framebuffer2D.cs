﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.Graphics.Rendering.Buffers
{
    /// <summary>
    /// Class representing a framebuffer with a 2D output texture.<br/>
    /// There is also a renderbuffer attached with <see cref="RenderbufferStorage.Depth24Stencil8"/>.
    /// </summary>
    public class Framebuffer2D : IBindable, IDisposable
    {

        private int _fbo;
        private int _rbo;

        /// <summary>
        /// Output texture of this framebuffer.
        /// </summary>
        public Texture2D OutputTexture { get; private set; } 


        public Framebuffer2D(int width, int height)
        {
            // Create frame buffer
            _fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

            // Create output texture
            Texture2DParameters parameters = new Texture2DParameters()
            {
                Width = width,
                Height = height,
                PixelInternalFormat = PixelInternalFormat.Rgb,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.UnsignedByte,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapModeS = TextureWrapMode.ClampToBorder,
                WrapModeT = TextureWrapMode.ClampToBorder,
                GenerateMipmaps = false
            };

            OutputTexture = new Texture2D(ref parameters);
            OutputTexture.Bind();


            // Bind output texture to framebuffer
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D,
                OutputTexture.Handle,
                0);


            // Create render buffer
            _rbo = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _fbo);

            GL.RenderbufferStorage(
                RenderbufferTarget.Renderbuffer,
                RenderbufferStorage.Depth24Stencil8,
                width,
                height);

            // Unbind RBO
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            // Attach rbo depth and stencil to fbo
            GL.FramebufferRenderbuffer(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthStencilAttachment,
                RenderbufferTarget.Renderbuffer, _rbo);



            // Check for completion
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Logger.Log(Severity.Error, "Failed to create framebuffer!");
            }
            else
            {
                Logger.Log(Severity.Info, $"Created framebuffer with size {width}x{height}");
            }

            // Unbind framebuffer.
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }


        /// <summary>
        /// Binds this framebuffer to the current renderer state.
        /// </summary>
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        }


        /// <summary>
        /// Unbind this framebuffer from the current renderer state (binds default framebuffer again).
        /// </summary>
        public void Unbind()
        {
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
                    OutputTexture.Dispose();
                }

                GL.DeleteFramebuffer(_fbo);
                _disposedValue = true;
            }
        }

        ~Framebuffer2D()
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