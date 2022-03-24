using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace VengineX.Graphics.Rendering.Vertices
{
    /// <summary>
    /// Vertex that is used for UI elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UIVertex
    {
        public Vector3 position;
        public Vector4 color;
        public Vector2 uvs;

        public override string ToString()
        {
            return $"pos: {position} col: {color} uvs: {uvs}";
        }
    }
}
