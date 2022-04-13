using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Math
{
    public struct BBox3i
    {
        public Vector3i Min { get; }
        public Vector3i Max { get; }
        public Vector3i Size { get; }
        public Vector3i HalfSize { get; }
        public Vector3i Center { get; }

        public BBox3i(Vector3i min, Vector3i max)
        {
            Min = min;
            Max = max;

            Size = max - min;
            HalfSize = Size / 2;
            Center = (max + min) / 2;
        }

        /// <summary>
        /// Checks if the given point is in these bounds.<br/>
        /// Minimum inclusive, maximum exclusive.
        /// </summary>
        public bool Contains(Vector3i point)
        {
            return point.X >= Min.X
                && point.X < Max.X
                && point.Y >= Min.Y
                && point.Y < Max.Y
                && point.Z >= Min.Z
                && point.Z < Max.Z;
        }
    }
}
