﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Vertices
{
    /// <summary>
    /// Vertex that is used for UI elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UIVertex
    {
        public Vector3 position;
        public Vector2 uv;
    }
}