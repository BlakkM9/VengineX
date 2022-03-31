﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.ECS;

namespace VengineX.Core
{
    /// <summary>
    /// Class that defines the position, rotation and scale of the entity it is attached to.
    /// </summary>
    public class Transform : Component
    {
        /// <summary>
        /// ModelMatrix of this transform, calculated from position, rotation and scale.
        /// </summary>
        public ref Matrix4 ModelMatrix => ref _modelMatrix;
        private Matrix4 _modelMatrix;

        /// <summary>
        /// Position of this transform
        /// </summary>
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateMatrix();
            } 
        }
        private Vector3 _position;

        /// <summary>
        /// Gets and sets the rotation of this transform (euler angels)
        /// </summary>
        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateMatrix();
            }
        }
        private Vector3 _rotation;

        /// <summary>
        /// Sets and gets the scale of this transform.
        /// </summary>
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateMatrix();
            }
        }
        private Vector3 _scale;


        /// <summary>
        /// Creates a new transform with given position, rotatio and scale.
        /// </summary>
        /// <param name="position">Position of the transform.</param>
        /// <param name="rotation">Rotation (euler angels) of this transform.</param>
        /// <param name="scale">Scale of the transform.</param>
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale) : base(typeof(Transform))
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            UpdateMatrix();
        }


        /// <summary>
        /// Creates a new transfrom with the matrix set to identity<br/>
        /// (Position 0, 0, 0; Rotation 0, 0, 0 and Scale 1, 1, 1).
        /// </summary>
        public Transform() : this(Vector3.Zero, Vector3.Zero, Vector3.One) { }


        /// <summary>
        /// Calculates <see cref="ModelMatrix"/> from <see cref="Position"/>, <see cref="Rotation"/> and <see cref="Scale"/>.
        /// </summary>
        private void UpdateMatrix()
        {
            Matrix4 rotation = Matrix4.CreateFromQuaternion(new Quaternion(Rotation));
            Matrix4 scale = Matrix4.CreateScale(Scale);
            Matrix4 translation = Matrix4.CreateTranslation(Position);
            _modelMatrix = scale * rotation * translation;
        }
    }
}
