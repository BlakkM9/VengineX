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
        public PerspectiveCamera(float fov, float viewportWidth, float viewportHeight, float nearPlane, float farPlane)
            : base(viewportWidth, viewportHeight)
        {

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov / 2.0f, viewportWidth / viewportHeight, nearPlane, farPlane);
        }
    }
}
