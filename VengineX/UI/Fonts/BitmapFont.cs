using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Buffers;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Graphics.Rendering.Vertices;
using VengineX.Resources;
using VengineX.Utils;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Fonts
{
    /// <summary>
    /// A font that is stored in a single texture and different letters are accessed by uvs.<br/>
    /// Font will not look good if it is scaled to much. SDF coming at some point, don't worry.
    /// </summary>
    public class BitmapFont : IDisposable, IResource, ILoadableResource
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ResourcePath { get; set; }

        /// <summary>
        /// TextureAtlas that was generated for this font.
        /// </summary>
        public Texture2D TextureAtlas { get; private set; }


        //private Dictionary<byte, Character> _characters;
        public Dictionary<byte, Texture2D?> _tex;

        /// <summary>
        /// Creates an empty BitmapFont object (for <see cref="ResourceManager"/>).
        /// </summary>
        public BitmapFont()
        {
            //_characters = new Dictionary<byte, Character>();
        }


        /// <summary>
        /// Loads a font from file by using freetype and given <see cref="BitmapFontLoadingParameters"/>.<br/>
        /// All filetypes that are supported by freetype are also working here.
        /// </summary>
        public void Load(ref ILoadingParameters loadingParameters)
        {
            BitmapFontLoadingParameters parameters = (BitmapFontLoadingParameters)loadingParameters;

            unsafe
            {
                // Load glyphs
                FreeTypeGlyph* glyphs = FreeTypeWrapper.LoadGlyphs(
                    parameters.FontPath,
                    parameters.FromCharCode,
                    parameters.ToCharCode,
                    parameters.Size);

                int length = parameters.ToCharCode - parameters.FromCharCode;

                // Create a texture2d for each character (if bitmap data is null, texture will be null aswell)
                //Dictionary<byte, Texture2D?> textures = CreateTextures(glyphs, length);
                _tex = CreateTextures(glyphs, length);

                // Render all the textures into a combined texture2d atlas.
                CreateAtlas(glyphs, length, parameters.Size, _tex);

                // Free glyphs
                FreeTypeWrapper.FreeGlyphs(glyphs, length);


                //Console.Write($"{(char)glyphs[i].charCode} ({glyphs[i].charCode})={(IntPtr)glyphs[i].bitmapData}, ");
                //Console.WriteLine($"charCode:   {glyphs[i].charCode}");
                //Console.WriteLine($"char:       {(char)glyphs[i].charCode}");
                //Console.WriteLine($"width:      {glyphs[i].width}");
                //Console.WriteLine($"height:     {glyphs[i].height}");
                //Console.WriteLine($"left:       {glyphs[i].left}");
                //Console.WriteLine($"top:        {glyphs[i].top}");
                //Console.WriteLine($"advance:    {glyphs[i].advance}");
                //Console.WriteLine($"data:       {(IntPtr)glyphs[i].bitmapData:X}\n");
            }
        }


        /// <summary>
        /// Creates <see cref="Texture2D"/>s of an unmanaged array holding <see cref="FreeTypeGlyph"/>s.<br/>
        /// If the bitmap data of any glyph is null, the texture will be null aswell.
        /// </summary>
        private static unsafe Dictionary<byte, Texture2D?> CreateTextures(FreeTypeGlyph* glyphs, int length)
        {
            Dictionary<byte, Texture2D?> textures = new Dictionary<byte, Texture2D?>();

            Texture2DParameters t2params = new Texture2DParameters()
            {
                PixelInternalFormat = PixelInternalFormat.R8,
                PixelFormat = PixelFormat.Red,
                PixelType = PixelType.UnsignedByte,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapModeS = TextureWrapMode.ClampToEdge,
                WrapModeT = TextureWrapMode.ClampToEdge,
                GenerateMipmaps = false,
            };


            // Disable byte-alignment restriction
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            for (int i = 0; i < length; i++)
            {
                FreeTypeGlyph glyph = glyphs[i];

                t2params.Width = glyph.width;
                t2params.Height = glyph.height;
                t2params.PixelData = (IntPtr)glyph.bitmapData;

                Texture2D? texture = glyph.bitmapData == null ? null : new Texture2D(ref t2params);
                textures.Add(glyph.charCode, texture);
            }

            return textures;
        }


        private unsafe void CreateAtlas(FreeTypeGlyph* glyphs, int length, int size, Dictionary<byte, Texture2D?> textures)
        {
            // Calculate the needed size of the atlas (squared and PoT) to fit all glyphs.
            int textureSize = MathUtils.CeilPoT(MathHelper.Sqrt((double)length * size * size));

            // Create framebuffer to render to.
            Framebuffer2D fb = new Framebuffer2D(textureSize, textureSize, PixelInternalFormat.R8, PixelFormat.Red, false);
            TextureAtlas = fb.DetachTexture();

            // Framebuffer no longer needed.
            fb.Dispose();

            Console.WriteLine($"{length} glyphs, each {size}x{size}: {textureSize}x{textureSize}");

        }


        /// <summary>
        /// Creates a mesh for the given text.
        /// </summary>
        public Mesh<UIVertex> CreateMesh(string text)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Calculates the width of the given text.
        /// </summary>
        public float CalculateWidth(string text)
        {
            throw new NotImplementedException();
        }


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
                    // Dispose managed
                    foreach (Texture2D tex in _tex.Values)
                    {
                        tex.Dispose();
                    }
                }

                // Dispose unmanaged

                // TODO: set large fields to null
                _disposedValue = true;
            }
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
