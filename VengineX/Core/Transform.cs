using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.ECS;

namespace VengineX.Core
{
    public class Transform : Component
    {
        public ref Matrix4 ModelMatrix { get => ref _modelMatrix; }
        private Matrix4 _modelMatrix;

        /// <summary>
        /// Creates a new transfrom with the matrix set to identity.
        /// </summary>
        public Transform()
        {
            _modelMatrix = Matrix4.Identity;
        }
    }
}
