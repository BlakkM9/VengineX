using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Cameras
{
    /// <summary>
    /// Camera with perspective view matrix
    /// </summary>
    public class PerspectiveCamera : Camera
    {
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
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov / 2.0f, width / height, nearPlane, farPlane);
        }
    }
}
