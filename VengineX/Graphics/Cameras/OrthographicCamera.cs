using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Cameras
{

    public class OrthographicCamera : Camera
    {
        /// <summary>
        /// Creates a new camera with orthographic projection matrix.<br/>
        /// If the camera is offCenter it might be used for an ui canvas.<br/>
        /// In that case bottom left is (0, 0) and top right (<paramref name="width"/>, <paramref name="height"/>) in this projection matrix.
        /// </summary>
        /// <param name="width">Width of the viewport</param>
        /// <param name="height">Height of the viewport</param>
        /// <param name="nearPlane">Near clipping plane</param>
        /// <param name="farPlane">Far clipping plane</param>
        /// <param name="offCenter">If set to true, the projection matrix is created off center.</param>
        public OrthographicCamera(float width, float height, float nearPlane, float farPlane, bool offCenter = false) : base()
        {
            if (offCenter)
            {
                ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, width, 0, height, nearPlane, farPlane);
            }
            else
            {
                ProjectionMatrix = Matrix4.CreateOrthographic(width, height, nearPlane, farPlane);
            }
        }
    }
}
