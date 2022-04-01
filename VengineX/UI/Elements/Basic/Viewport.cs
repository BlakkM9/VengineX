using OpenTK.Mathematics;
using System.Drawing;
using VengineX.Graphics.Renderers;

namespace VengineX.UI.Elements.Basic
{
    /// <summary>
    /// Every (part of a) child that is outside of this element will not be rendered.<br/>
    /// </summary>
    public class Viewport : Element
    {

        public Viewport(Element? parent) : base(parent) { }


        public override IEnumerable<QuadVertex> EnumerateQuads()
        {
            RectangleF viewportRect = new RectangleF(AbsolutePosition.X, AbsolutePosition.Y, Width, Height);

            foreach (Element child in EnumerateChildren())
            {
                if (!child.Visible) { continue; }

                foreach (QuadVertex quad in child.EnumerateQuads())
                {
                    RectangleF childRect = new RectangleF(quad.position.X, quad.position.Y, quad.size.X, quad.size.Y);


                    if (viewportRect.Contains(childRect))
                    {
                        // Child is completely contained in viewport
                        yield return quad;
                    }
                    else if (viewportRect.IntersectsWith(childRect))
                    {
                        // Child is partly in viewport, adjust quad
                        RectangleF intersection = RectangleF.Intersect(viewportRect, childRect);

                        Vector2[] remappedUVs = RemapUVs(quad.uv0, quad.uv1, quad.uv2, childRect, intersection);

                        yield return new QuadVertex()
                        {
                            position = new Vector2(intersection.X, intersection.Y),
                            size = new Vector2(intersection.Width, intersection.Height),
                            uv0 = remappedUVs[0],
                            uv1 = remappedUVs[1],
                            uv2 = remappedUVs[2],
                            uv3 = remappedUVs[3],
                            color = quad.color,
                            texture = quad.texture,
                        };

                    }

                    // Child is not in viewport, skip quad
                }
            }
        }


        private static Vector2[] RemapUVs(Vector2 uv0, Vector2 uv1, Vector2 uv2, RectangleF original, RectangleF cropped)
        {
            bool cutLeft = cropped.X > original.X;
            bool cutRight = cropped.X + cropped.Width < original.X + original.Width;
            bool cutTop = !(cropped.Y > original.Y);
            bool cutBottom = !(cropped.Y + cropped.Height < original.Y + original.Height);

            float dW = cropped.Width / original.Width;
            float dH = cropped.Height / original.Height;

            float uvX1 = uv0.X;
            float uvX2 = uv2.X;
            float uvY1 = uv0.Y;
            float uvY2 = uv1.Y;

            if (cutLeft)
            {
                uvX1 = MathHelper.Lerp(uv2.X, uv0.X, dW);
            }
            else if (cutRight)
            {
                uvX2 = MathHelper.Lerp(uv0.X, uv2.X, dW);
            }

            if (cutTop)
            {
                uvY1 = MathHelper.Lerp(uv1.Y, uv0.Y, dH);
            }
            else if (cutBottom)
            {
                uvY2 = MathHelper.Lerp(uv0.Y, uv1.Y, dH);
            }


            return new Vector2[]
            {
                new Vector2(uvX1, uvY1),
                new Vector2(uvX1, uvY2),
                new Vector2(uvX2, uvY1),
                new Vector2(uvX2, uvY2),
            };
        }
    }
}
