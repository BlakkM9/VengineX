using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Resources;

namespace VengineX.Wrappers.FreeType
{

    // TODO Make ILoadableResource
    // TODO pack all glyphs on a single texture and save uv's
    public class FreeTypeFont : IDisposable
    {


        /// <summary>
        /// Dictionary holding all loaded character.
        /// Key is byte that represents ASCII char code.
        /// </summary>
        public Dictionary<byte, Character> Characters;

        public FreeTypeFont(string filePath, byte fromCharCode, byte toCharCode, int size)
        {

            Characters = new Dictionary<byte, Character>();

            unsafe
            {
                // Load glyphs
                Glyph* glyphs = FreeTypeWrapper.LoadGlyphs(filePath, fromCharCode, toCharCode, size);
                int length = toCharCode - fromCharCode;


                // Create Characters (textures for glyphs)
                // disable byte-alignment restriction
                GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);


                for (int i = 0; i < length; i++)
                {
                    Glyph glyph = glyphs[i];


                    int texture = 0;
                    if (glyph.bitmapData != null)
                    {
                        // Generate texture if glyph has bitmap data
                        texture = GL.GenTexture();
                        GL.BindTexture(TextureTarget.Texture2D, texture);


                        GL.TexImage2D(
                            TextureTarget.Texture2D,
                            0,
                            PixelInternalFormat.R8,
                            glyph.width,
                            glyph.height,
                            0,
                            PixelFormat.Red,
                            PixelType.UnsignedByte,
                            (IntPtr)glyph.bitmapData
                        );


                        // Set texture parameters
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                    }


                    // Store character for later use
                    Character character = new Character()
                    {
                        texture = texture,
                        size = new Vector2i(glyph.width, glyph.height),
                        bearing = new Vector2i(glyph.left, glyph.top),
                        advance = glyph.advance
                    };


                    if (!Characters.TryAdd(glyph.charCode, character))
                    {
                        // Failed to add
                        Logger.Log(Severity.Error, Tag.Loading, $"Failed to add Character with char code {glyph.charCode}, was already present");
                        // Delete texture if it was created
                        if (texture != 0) { GL.DeleteTexture(texture); }
                    }

                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }

                // Free glyphs and ft lib
                FreeTypeWrapper.FreeGlyphs(glyphs, length);

                // TODO don't do that every time
                FreeTypeWrapper.FreeFreeType();
            }
        }


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


                foreach (Character c in Characters.Values)
                {
                    GL.DeleteTexture(c.texture);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FreeTypeFont()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
