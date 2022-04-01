using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace VengineX.Graphics.Vertices
{
    /// <summary>
    /// Basic vertex for meshes that use a pbr or comparable shader.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PBRVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector3 tangent;
        public Vector3 uv;
        public Vector4 color;
    }
}
