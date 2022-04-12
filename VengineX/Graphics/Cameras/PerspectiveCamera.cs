using OpenTK.Mathematics;

namespace VengineX.Graphics.Cameras
{
    /// <summary>
    /// Camera with perspective projection.
    /// </summary>
    public class PerspectiveCamera : Camera
    {
        /// <summary>
        /// FOV of this camera in degree.
        /// </summary>
        public float FieldOfView { get; }

        /// <summary>
        /// Width of the camera viewport.
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Height of the camera viewport.
        /// </summary>
        public float Height { get; }


        /// <summary>
        /// Creates a camera with perspective projection matrix.
        /// </summary>
        /// <param name="fov">Field of view in radians.</param>
        /// <param name="width">Width of the viewport</param>
        /// <param name="height">Height of the viewport</param>
        /// <param name="nearPlane">Near clipping plane</param>
        /// <param name="farPlane">Far clipping plane</param>
        public PerspectiveCamera(float fov, float width, float height, float nearPlane, float farPlane)
            : base()
        {
            FieldOfView = fov;
            Width = width;
            Height = height;

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov / 2.0f, width / height, nearPlane, farPlane);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void SetClippingPlanes(float nearPlane, float farPlane)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FieldOfView / 2.0f, Width / Height, nearPlane, farPlane);
            Update();
        }
    }
}
