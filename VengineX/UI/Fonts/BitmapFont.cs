using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Buffers;
using VengineX.Graphics.Cameras;
using VengineX.Graphics.Renderers;
using VengineX.Graphics.Shaders;
using VengineX.Graphics.Textures;
using VengineX.Resources;
using VengineX.Utils;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Fonts
{
    /// <summary>
    /// A font that is stored in a single texture and different letters are accessed by uvs.<br/>
    /// Font will not look good if it is scaled to much. SDF coming at some point, don't worry.
    /// </summary>
    public class BitmapFont : Font
    {
        /// <summary>
        /// TextureAtlas that was generated for this font.
        /// </summary>
        public Texture2D? TextureAtlas { get; private set; }


        /// <summary>
        /// Loads a font from file by using freetype and given <see cref="BitmapFontLoadingParameters"/>.<br/>
        /// All filetypes that are supported by freetype are also working here.
        /// </summary>
        public override void Load(ref ILoadingParameters loadingParameters)
        {
            BitmapFontLoadingParameters parameters = (BitmapFontLoadingParameters)loadingParameters;
            Size = parameters.Size;

            unsafe
            {
                // Load glyphs
                FreeTypeGlyph* glyphs = FreeTypeWrapper.LoadGlyphs(
                    parameters.FontPath,
                    parameters.Ranges,
                    parameters.Size);

                int length = 0;
                foreach (CharacterRange range in parameters.Ranges)
                {
                    length += range.to - range.from;
                }
                Logger.Log("Total of " + length + " characters loaded");

                // Create a texture2d for each character (if bitmap data is null, texture will be null aswell)
                Dictionary<char, Texture2D?> textures = CreateTextures(glyphs, length);

                // Render all the textures into a combined texture2d atlas.
                CreateAtlas(glyphs, length, parameters.Size, textures);


                // Dispose temporary textures
                foreach (Texture2D? texture in textures.Values)
                {
                    texture?.Dispose();
                }

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
        private static unsafe Dictionary<char, Texture2D?> CreateTextures(FreeTypeGlyph* glyphs, int length)
        {
            Dictionary<char, Texture2D?> textures = new Dictionary<char, Texture2D?>();

            Texture2DParameters t2params = new Texture2DParameters()
            {
                InternalFormat = SizedInternalFormat.R8,
                PixelFormat = PixelFormat.Red,
                PixelType = PixelType.UnsignedByte,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapModeS = TextureWrapMode.Repeat,
                WrapModeT = TextureWrapMode.Repeat,
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


        private unsafe void CreateAtlas(FreeTypeGlyph* glyphs, int length, int size, Dictionary<char, Texture2D?> textures)
        {
            // Calculate the needed size of the atlas (squared and PoT) to fit all glyphs.
            int textureSize = MathUtils.CeilPoT(MathHelper.Sqrt((double)length * size * size));

            // Create framebuffer to render to.
            Framebuffer2D fb = new Framebuffer2D(textureSize, textureSize, SizedInternalFormat.Rgba8, PixelFormat.Rgba, false);
            TextureAtlas = fb.DetachTexture();

            // Get shader, create quad and uniforms required for rendering
            Shader bmpFontShader = ResourceManager.GetResource<Shader>("shader.ui.bmpfont");


            // Render all the textures
            fb.Bind();
            BatchRenderer2D br = new BatchRenderer2D(1000, bmpFontShader);
            fb.Clear(ClearBuffer.Color, new float[] { 0, 0, 0, 0 });

            OrthographicCamera camera = new OrthographicCamera(textureSize, textureSize, -1, 1, true);

            int rowSpaceUsed = 0;
            int x = 0;
            int y = 0;

            br.Begin(camera);

            for (int i = 0; i < length; i++)
            {
                FreeTypeGlyph glyph = glyphs[i];

                float w = glyph.width;
                float h = glyph.height;


                QuadVertex q = new QuadVertex();
                q.position = new Vector2(x, textureSize - y - h);
                q.size = new Vector2(w, h);
                q.texture = textures[glyph.charCode];
                q.uv0 = new Vector2(0, 0);
                q.uv1 = new Vector2(0, 1);
                q.uv2 = new Vector2(1, 0);
                q.uv3 = new Vector2(1, 1);
                br.Submit(q);


                // Calculate uvs: upper-left, upper-right, lower-left, lower-right.
                Vector2[] uvs = new Vector2[]
                {
                    new Vector2((float)x / textureSize,         1 - (float)y / textureSize),
                    new Vector2((float)x / textureSize,         1 - (float)(y + h) / textureSize),
                    new Vector2((float)(x + w) / textureSize,   1 - (float)y / textureSize),
                    new Vector2((float)(x + w) / textureSize,   1 - (float)(y + h) / textureSize),
                };


                // Create Character and add to dict.
                Characters.Add(glyph.charCode, new Character()
                {
                    HasTexture = textures[glyph.charCode] != null,
                    Size = new Vector2i(glyph.width, glyph.height),
                    Bearing = new Vector2i(glyph.left, glyph.top),
                    Advance = glyph.advance,
                    UVs = uvs,
                });


                // Don't update to next pos if no texture was rendered in that position.
                if (!Characters[glyph.charCode].HasTexture) { continue; }

                // Calculate next x and y.
                rowSpaceUsed += size;
                if (rowSpaceUsed + size > textureSize)
                {
                    // No more space in this row.
                    y += size;
                    x = 0;
                    rowSpaceUsed = 0;
                }
                else
                {
                    x += size;
                }
            }


            br.End();
            br.Flush();

            // Dispose stuff that is no longer needed.
            br.Dispose();
            fb.Unbind();
            fb.Dispose();
        }


        public QuadVertex[] CreateQuads(string text, Vector2 position, float size, Vector4 color)
        {
            // Calculate quad count
            int quadCount = 0;
            foreach (char c in text)
            {
                Character ch = Characters[c];
                if (ch.HasTexture) { quadCount++; }
            }

            QuadVertex[] quads = new QuadVertex[quadCount];
            int index = 0;
            float scale = size / Size;
            float x = position.X;

            foreach (char c in text)
            {
                Character ch = Characters[c];

                // Ignore non texture chars
                if (ch.HasTexture)
                {
                    // Dimensions
                    float xPos = x + ch.Bearing.X * scale;
                    float yPos = position.Y - (ch.Size.Y - ch.Bearing.Y) * scale;

                    float w = ch.Size.X * scale;
                    float h = ch.Size.Y * scale;


                    quads[index++] = new QuadVertex()
                    {
                        position = new Vector2(xPos, yPos),
                        size = new Vector2(w, h),
                        uv0 = ch.UVs[0],
                        uv1 = ch.UVs[1],
                        uv2 = ch.UVs[2],
                        uv3 = ch.UVs[3],
                        texture = TextureAtlas,
                        color = color,
                    };
                }

                x += (ch.Advance >> 6) * scale;
            }

            return quads;
        }


        #region IDisposable

        private bool _disposedValue;

        /// <summary>
        /// Disposable pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    TextureAtlas?.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }

        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Font()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        /// <summary>
        /// Disposable pattern
        /// </summary>
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
