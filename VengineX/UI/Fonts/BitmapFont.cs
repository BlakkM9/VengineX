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
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Graphics.Rendering.UnitModels;
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

        /// <summary>
        /// The size (height) of the font in pixels.
        /// </summary>
        public float Size { get; private set; }

        public Dictionary<byte, Texture2D?> textures;


        private readonly Dictionary<byte, Character> _characters;

        /// <summary>
        /// Creates an empty BitmapFont object (for <see cref="ResourceManager"/>).
        /// </summary>
        public BitmapFont()
        {
            _characters = new Dictionary<byte, Character>();
        }


        /// <summary>
        /// Loads a font from file by using freetype and given <see cref="BitmapFontLoadingParameters"/>.<br/>
        /// All filetypes that are supported by freetype are also working here.
        /// </summary>
        public void Load(ref ILoadingParameters loadingParameters)
        {
            BitmapFontLoadingParameters parameters = (BitmapFontLoadingParameters)loadingParameters;
            Size = parameters.Size;

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
                textures = CreateTextures(glyphs, length);

                // Render all the textures into a combined texture2d atlas.
                CreateAtlas(glyphs, length, parameters.Size, textures);


                //// Dispose temporary textures
                //foreach (Texture2D? texture in textures.Values)
                //{
                //    texture?.Dispose();
                //}

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

            // Get shader, create quad and uniforms required for rendering
            Quad quad = new Quad();
            Shader imageShader = ResourceManager.GetResource<Shader>("shader.image");
            Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, textureSize, -textureSize, 0, -1.0f, 1.0f);
            Matrix4 view = Matrix4.Identity;
            Vector4 color = Vector4.One;

            // Render all the textures
            fb.Bind();
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            imageShader.Bind();
            imageShader.SetUniformMat4("P", ref proj);
            imageShader.SetUniformMat4("V", ref view);
            imageShader.SetUniformVec4("uColor", ref color);


            int rowSpaceUsed = 0;
            int x = 0;
            int y = 0;
            for (int i = 0; i < length; i++)
            {
                FreeTypeGlyph glyph = glyphs[i];

                // Update model matrix.
                Matrix4 m = Matrix4.Identity;
                m *= Matrix4.CreateScale(glyph.width / 2f, glyph.height / -2f, 0);
                m *= Matrix4.CreateTranslation(glyph.width / 2f + x, -(glyph.height / 2f + y), 0);
                imageShader.SetUniformMat4("M", ref m);

                // Bind the glyphs texture
                textures[glyph.charCode]?.Bind();

                // Render on quad.
                quad.Render();


                float w = glyph.width;
                float h = glyph.height;

                // Calculate uvs: upper-left, upper-right, lower-left, lower-right.
                Vector2[] uvs = new Vector2[]
                {
                    new Vector2((float)x / textureSize,         (float)(y + h) / textureSize),
                    new Vector2((float)(x + w) / textureSize,   (float)(y + h) / textureSize),
                    new Vector2((float)x / textureSize,         (float)y / textureSize),
                    new Vector2((float)(x + w) / textureSize,   (float)y / textureSize),
                };


                // Create Character and add to dict.
                _characters.Add(glyph.charCode, new Character()
                {
                    Size = new Vector2i(glyph.width, glyph.height),
                    Bearing = new Vector2i(glyph.left, glyph.top),
                    Advance = glyph.advance,
                    UVs = uvs,
                });


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


            fb.Unbind();


            // Dispose stuff that is no longer needed.
            fb.Dispose();
            quad.Dispose();
        }


        /// <summary>
        /// Creates a mesh for the given text.
        /// </summary>
        public Mesh<UIVertex> CreateMesh(string text)
        {
            UIVertex[] vertices = new UIVertex[text.Length * 4];
            uint[] indices = new uint[text.Length * 6];

            uint x = 0;
            uint vertIndex = 0;
            uint indIndex = 0;

            foreach (char c in text)
            {
                Character ch = _characters[(byte)c];

                // Dimensions
                float xPos = x + ch.Bearing.X;
                float yPos = -(ch.Size.Y - ch.Bearing.Y);

                float w = ch.Size.X;
                float h = ch.Size.Y;

                // Vertices
                vertices[vertIndex + 0] = new UIVertex()
                {
                    position = new Vector3(xPos, yPos, 0),
                    uv = ch.UVs[0],
                };
                vertices[vertIndex + 1] = new UIVertex()
                {
                    position = new Vector3(xPos + w, yPos, 0),
                    uv = ch.UVs[1],
                };
                vertices[vertIndex + 2] = new UIVertex()
                {
                    position = new Vector3(xPos, yPos + h, 0),
                    uv = ch.UVs[2],
                };
                vertices[vertIndex + 3] = new UIVertex()
                {
                    position = new Vector3(xPos + w, yPos + h, 0),
                    uv = ch.UVs[3],
                };

                Console.WriteLine(vertices[vertIndex + 0].position + ", " + vertices[vertIndex + 0].uv);
                Console.WriteLine(vertices[vertIndex + 1].position + ", " + vertices[vertIndex + 1].uv);
                Console.WriteLine(vertices[vertIndex + 2].position + ", " + vertices[vertIndex + 2].uv);
                Console.WriteLine(vertices[vertIndex + 3].position + ", " + vertices[vertIndex + 3].uv);

                // Indices
                indices[indIndex + 0] = vertIndex + 0;
                indices[indIndex + 1] = vertIndex + 1;
                indices[indIndex + 2] = vertIndex + 2;

                indices[indIndex + 3] = vertIndex + 2;
                indices[indIndex + 4] = vertIndex + 1;
                indices[indIndex + 5] = vertIndex + 3;

                vertIndex += 4;
                indIndex += 6;

                x += ch.Advance >> 6;
            }

            return new Mesh<UIVertex>(Vector3.Zero, vertices, indices);
        }


        /// <summary>
        /// Calculates the width of the given text (unscaled).
        /// </summary>
        public float CalculateWidth(string text)
        {
            float width = 0;
            foreach (char c in text)
            {
                width += _characters[(byte)c].Advance;

            }
            return width;
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
                    TextureAtlas.Dispose();

                    // Dispose temporary textures
                    foreach (Texture2D? texture in textures.Values)
                    {
                        texture?.Dispose();
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
