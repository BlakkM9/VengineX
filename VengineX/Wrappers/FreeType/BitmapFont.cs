using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Resources;

namespace VengineX.Wrappers.FreeType
{
    /// <summary>
    /// A font that is stored in a single texture and different letters are accessed by uvs.
    /// </summary>
    public class BitmapFont : IDisposable, IResource, ILoadableResource
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ResourcePath { get; set; }

        public Texture2D TextureAtlas { get; private set; }

        public BitmapFont() { }


        public void Load(ref ILoadingParameters loadingParameters)
        {
            throw new NotImplementedException();
        }


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    TextureAtlas?.Dispose();
                }

                
                // TODO: set large fields to null
                _disposedValue = true;
            }
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
