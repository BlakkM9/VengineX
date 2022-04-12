using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Math
{
    /// <summary>
    /// Class represending a bounding box in 3d space with integers.
    /// </summary>
    public struct BBox3i
    {
        /// <summary>
        /// Inclusive minimum.
        /// </summary>
        public Vector3i Min { get; }

        /// <summary>
        /// Exclusive maximum.
        /// </summary>
        public Vector3i Max { get; }

        /// <summary>
        /// The center of this bounding box.
        /// </summary>
        public Vector3i Center { get; }

        /// <summary>
        /// The size of this bounding box.
        /// </summary>
        public Vector3i Size { get; }


        /// <summary>
        /// Creates a new bounding box.
        /// </summary>
        /// <param name="min">Inclusive minimum.</param>
        /// <param name="max">Exclusive maximum.</param>
        public BBox3i(Vector3i min, Vector3i max)
        {
            Min = min;
            Max = max;
            Size = max - min;
            Center = (max + min) / 2;
        }


        /// <summary>
        /// Creates a new bounding box.
        /// </summary>
        /// <param name="min">Inclusive minimum.</param>
        /// <param name="max">Exclusive maximum.</param>
        public BBox3i(Vector3 min, Vector3 max) : this((Vector3i)min, (Vector3i)max) { }


        /// <summary>
        /// Checks if the given point is in these bounds.
        /// </summary>
        public bool Contains(Vector3 point) => Contains((Vector3i)point);


        /// <summary>
        /// Checks if the given point is in these bounds.
        /// </summary>
        public bool Contains(Vector3i point) => point.X >= Min.X
            && point.Y >= Min.Y
            && point.X < Max.X
            && point.Y < Max.Y
            && point.Z >= Min.Z
            && point.Z < Max.Z;
    }
}
