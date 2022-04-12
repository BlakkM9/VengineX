using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Utils.Extensions
{
    public static class MatrixUtils
    {
        public static Matrix4d CreateScale(this Matrix4d matrix, Vector3d scale)
        {
            matrix = Matrix4d.Identity;
            matrix.Row0.X = scale.X;
            matrix.Row1.Y = scale.Y;
            matrix.Row2.Z = scale.Z;

            return matrix;
        }


        /// <summary>
        /// Returns a copy of this Matrix4d with single precision matrix4.
        /// </summary>
        public static Matrix4 ToMatrix4(this Matrix4d matrix)
        {
            Matrix4 mat4 = Matrix4.Identity;

            mat4.Row0 = (Vector4)matrix.Row0;
            mat4.Row1 = (Vector4)matrix.Row1;
            mat4.Row2 = (Vector4)matrix.Row2;
            mat4.Row3 = (Vector4)matrix.Row3;

            return mat4;
        }
    }
}
