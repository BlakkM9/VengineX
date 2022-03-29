using OpenTK.Mathematics;
using System.Drawing;
using VengineX.Graphics.Rendering.Renderers;

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

                        bool cutLeft = intersection.X > quad.position.X;
                        bool cutRight = intersection.X + intersection.Width < quad.position.X + quad.size.X;
                        bool cutTop = !(intersection.Y > quad.position.Y);
                        bool cutBottom = !(intersection.Y + intersection.Height < quad.position.Y + quad.size.Y);

                        float dW = intersection.Width / childRect.Width;
                        float dH = intersection.Height / childRect.Height;

                        float uvX1 = quad.uv0.X;
                        float uvX2 = quad.uv2.X;
                        float uvY1 = quad.uv0.Y;
                        float uvY2 = quad.uv1.Y;

                        if (cutLeft)
                        {
                            uvX1 = MathHelper.Lerp(quad.uv2.X, quad.uv0.X, dW);
                        }
                        else if (cutRight)
                        {
                            uvX2 = MathHelper.Lerp(quad.uv0.X, quad.uv2.X, dW);
                        }

                        if (cutTop)
                        {
                            uvY1 = MathHelper.Lerp(quad.uv1.Y, quad.uv0.Y, dH);
                        }
                        else if (cutBottom)
                        {
                            uvY2 = MathHelper.Lerp(quad.uv0.Y, quad.uv1.Y, dH);
                        }

                        yield return new QuadVertex()
                        {
                            position = new Vector2(intersection.X, intersection.Y),
                            size = new Vector2(intersection.Width, intersection.Height),
                            uv0 = new Vector2(uvX1, uvY1),
                            uv1 = new Vector2(uvX1, uvY2),
                            uv2 = new Vector2(uvX2, uvY1),
                            uv3 = new Vector2(uvX2, uvY2),
                            color = quad.color,
                            texture = quad.texture,
                        };

                    }

                    // Child is not in viewport, skip quad
                }
            }
        }
    }
}
