using VengineX.Graphics.Textures;

namespace VengineX.Graphics.Buffers
{
    public abstract class Framebuffer<T> where T : Texture
    {
        protected uint _fbo;


        /// <summary>
        /// Output texture of this framebuffer.
        /// </summary>
        public T OutputTexture { get; protected set; }

        /// <summary>
        /// Wether or not the output texture is detached from this framebuffer.<br/>
        /// If the texture is detached it will no longer be disposed if the framebuffer is disposed.
        /// </summary>
        public bool IsTextureDetached { get; protected set; } = false;

        /// <summary>
        /// Holds the viewport dimensions before binding this framebuffer (and chaning viewport to fb's size)
        /// </summary>
        protected int[] _viewportBeforeBind = new int[4];


        /// <summary>
        /// Detaches the output texture of this framebuffer.<br/>
        /// A detached texture will no longer be disposed when the framebuffer is disposed.
        /// </summary>
        /// <returns>Detached Texture2D</returns>
        public virtual T DetachTexture()
        {
            IsTextureDetached = true;
            return OutputTexture;
        }
    }
}
