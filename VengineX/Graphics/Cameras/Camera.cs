using OpenTK.Mathematics;
using VengineX.Core;
using VengineX.Debugging.Logging;
using VengineX.ECS;

namespace VengineX.Graphics.Cameras
{
    /// <summary>
    /// Base class for camera. Provides all required matrices and basic function aswell as a frustum that<br/>
    /// might be used for frustum culling.
    /// </summary>
    public abstract class Camera : Component
    {
        /// <summary>
        /// View matrix of this camera (Transforms from world to camera space)
        /// </summary>
        public ref Matrix4 ViewMatrix
        {
            get { return ref _viewMatrix; }
        }
        private Matrix4 _viewMatrix;


        /// <summary>
        /// Projection matrix of this camera (Transforms from camera space to clip space)
        /// </summary>
        public ref Matrix4 ProjectionMatrix
        {
            get { return ref _projectionMatrix; }
        }
        private Matrix4 _projectionMatrix;

        /// <summary>
        /// Transform of the camera, received from parent entity.
        /// </summary>
        public Transform? Transform { get; private set; }

        /// <summary>
        /// The frustum of this camera.
        /// </summary>
        public Frustum Frustum { get; }


        /// <summary>
        /// Creates a new camera at given position.
        /// </summary>
        public Camera() : base(typeof(Camera))
        {
            _viewMatrix = Matrix4.Identity;
            Frustum = new Frustum();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Attached()
        {
            Transform = Entity?.GetComponent<Transform>();

            if (Transform == null)
            {
                Logger.Log(Severity.Fatal, "A mesh can only be attached to components that have a Transform component attached already!");
            }

            Update();
            Transform.TransformChanged += (_) => Update();
        }


        /// <summary>
        /// Recalculates the view matrix based on the transform of the parent entity.<br/>
        /// Automatically called when the transform changed.
        /// </summary>
        private void Update()
        {
            Matrix4 rotation = Matrix4.CreateFromQuaternion(new Quaternion(Transform.Rotation));
            Matrix4 translation = Matrix4.CreateTranslation(-Transform.Position);
            _viewMatrix = translation * rotation;
            Frustum.CalculateFrustum(_projectionMatrix, _viewMatrix);
        }
    }
}
