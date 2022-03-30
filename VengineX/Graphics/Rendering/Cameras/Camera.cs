using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Cameras
{
    /// <summary>
    /// Base class for camera. Provides all required matrices and basic function aswell as a frustum that<br/>
    /// might be used for frustum culling.
    /// </summary>
    public abstract class Camera
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
        /// World space position of this camera
        /// </summary>
        public ref Vector3 Position
        {
            get { return ref _position; }
        }
        private Vector3 _position;


        /// <summary>
        /// The frustum of this camera.
        /// </summary>
        public Frustum Frustum { get; }


        /// <summary>
        /// Creates a new camera at given position.
        /// </summary>
        public Camera(Vector3 position)
        {
            Position = position;
            _viewMatrix = Matrix4.Identity;
            Frustum = new Frustum();

            Update();
        }

        /// <summary>
        /// Needs to be called after translation/positon change or rotation so view matrix is updated
        /// </summary>
        public virtual void Update()
        {
            _viewMatrix *= Matrix4.CreateTranslation(Position);
            Frustum.CalculateFrustum(_projectionMatrix, _viewMatrix);
        }

        /// <summary>
        /// Moves the camera by given translation
        /// </summary>
        public void Translate(Vector3 translation)
        {
            _position += translation;
        }
    }
}
