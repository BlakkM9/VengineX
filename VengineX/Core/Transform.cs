using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.ECS;
using VengineX.Utils.Extensions;

namespace VengineX.Core
{
    /// <summary>
    /// Class that defines the position, rotation and scale of the entity it is attached to.
    /// </summary>
    public class Transform : Component
    {
        /// <summary>
        /// Occurs when the transform changed.
        /// </summary>
        public event Action<Transform>? TransformChanged;

        /// <summary>
        /// ModelMatrix of this transform, calculated from position, rotation and scale.
        /// </summary>
        public ref Matrix4d ModelMatrix => ref _modelMatrix;
        private Matrix4d _modelMatrix;

        /// <summary>
        /// Position of this transform
        /// </summary>
        public Vector3d Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateMatrix();
            } 
        }
        private Vector3d _position;

        /// <summary>
        /// Gets and sets the rotation of this transform in radians (euler angles)
        /// </summary>
        public Vector3d Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateMatrix();
            }
        }
        private Vector3d _rotation;

        /// <summary>
        /// Sets and gets the scale of this transform.
        /// </summary>
        public Vector3d Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateMatrix();
            }
        }
        private Vector3d _scale;


        /// <summary>
        /// The vector of that transform that points forwards.
        /// </summary>
        public Vector3 Forward { get; private set; }



        /// <summary>
        /// Creates a new transform with given position, rotatio and scale.
        /// </summary>
        /// <param name="position">Position of the transform.</param>
        /// <param name="rotation">Rotation (euler angles) of this transform.</param>
        /// <param name="scale">Scale of the transform.</param>
        public Transform(Vector3d position, Vector3d rotation, Vector3d scale) : base(typeof(Transform))
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
        public Transform() : this(Vector3d.Zero, Vector3d.Zero, Vector3d.One)
        {
            UpdateMatrix();
        }


        /// <summary>
        /// Calculates <see cref="ModelMatrix"/> from <see cref="Position"/>, <see cref="Rotation"/> and <see cref="Scale"/>.
        /// </summary>
        private void UpdateMatrix()
        {
            Matrix4d rotation = Matrix4d.CreateFromQuaternion(new Quaterniond(Rotation));
            Matrix4d scale = Matrix4d.Identity.CreateScale(Scale);
            Matrix4d translation = Matrix4d.CreateTranslation(Position);
            _modelMatrix = scale * rotation * translation;


            Vector3 front = Vector3.Zero;
            front.X = (float)(MathHelper.Cos(Rotation.Y) * MathHelper.Cos(Rotation.X));
            front.Y = (float)(MathHelper.Cos(Rotation.Y) * MathHelper.Sin(Rotation.X));
            front.Z = (float)(MathHelper.Sin(Rotation.Y));
            Forward = front.Normalized();

            TransformChanged?.Invoke(this);
        }


        /// <summary>
        /// Rotates this transform by the given vector (euler angles, radiant, relative rotation).
        /// </summary>
        public void Rotate(Vector3 rotation)
        {
            Rotation += rotation;
        }


        /// <summary>
        /// Moves the transform by the given vector (relative).
        /// </summary>
        public void Move(Vector3 moveAmount)
        {
            Position += moveAmount;
        }
    }
}
