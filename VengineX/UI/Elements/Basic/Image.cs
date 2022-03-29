using OpenTK.Mathematics;
using VengineX.Graphics.Rendering.Renderers;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.UI.Elements.Basic
{
    /// <summary>
    /// Class representing an image on the ui.<br/>
    /// This is pretty much always used when there is something to render on the ui.
    /// </summary>
    public class Image : Element
    {
        /// <summary>
        /// The color that is used if there is no texture.<br/>
        /// This should be <see cref="Vector4.Zero"/> if there is any texture present.<br/>
        /// If you want to tint the texture use <see cref="Tint"/> instead.
        /// </summary>
        public Vector4 Color { get => _color; set => _color = value; }
        private Vector4 _color = Vector4.Zero;

        /// <summary>
        /// Tint of this image.
        /// </summary>
        public Vector4 Tint { get => _tint; set => _tint = value; }
        private Vector4 _tint = Vector4.One;

        /// <summary>
        /// The texture of this image.
        /// </summary>
        public Texture2D? Texture { get; set; } = null;


        /// <summary>
        /// Constructor for creating instance with <see cref="UISerializer"/>.
        /// </summary>
        public Image(Element parent) : base(parent) { }

        /// <summary>
        /// Creates a new image ui element from given parameters.
        /// </summary>
        public Image(Element parent, Texture2D? texture, Vector4 color, Vector4 tint)
            : this(parent)
        {
            Color = color;
            Tint = tint;
            Texture = texture;
        }

        /// <summary>
        /// Overload for <see cref="Image(Element, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(Element parent, Vector4 color)
            : this(parent, null, color, new Vector4(1, 1, 1, 0)) { }

        /// <summary>
        /// Overload for <see cref="Image(Element, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(Element parent, Texture2D texture, Vector4 tint)
            : this(parent, texture, Vector4.Zero, tint) { }

        /// <summary>
        /// Overload for <see cref="Image(Element, Texture2D, Vector4, Vector4)"/>.
        /// </summary>
        public Image(Element parent, Texture2D texture)
            : this(parent, texture, Vector4.Zero, Vector4.One) { }


        public override IEnumerable<QuadVertex> EnumerateQuads()
        {
            // Self
            QuadVertex q = new QuadVertex();
            q.position = new Vector2(AbsolutePosition.X, Canvas.Height - AbsolutePosition.Y - Height);
            q.size = Size;
            q.texture = Texture;
            q.color = Texture == null ? Color : Tint;
            yield return q;


            // Children
            foreach (Element child in EnumerateChildren())
            {
                foreach (QuadVertex quadVertex in child.EnumerateQuads())
                {
                    yield return quadVertex;
                }
            }
        }
    }
}
