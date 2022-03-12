using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Vertices
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
