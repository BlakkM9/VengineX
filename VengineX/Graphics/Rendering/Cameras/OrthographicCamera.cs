using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Cameras
{

    public class OrthographicCamera : Camera
    {

        public OrthographicCamera(Vector3 position, float viewportWidth, float viewportHeight, float nearPlane, float farPlane)
            : base(position)
        {
            ProjectionMatrix = Matrix4.CreateOrthographic(viewportWidth, viewportHeight, nearPlane, farPlane);
        }


        /// <summary>
        /// Creates a orthographic projection matrix off center.<br/>
        /// This constructor might be used for an ui canvas.<br/>
        /// Bottom left is 0, 0 and top right width, height in this projection matrix.
        /// </summary>
        /// <param name="width">Width of the viewport</param>
        /// <param name="height">Height of the viewport</param>
        /// <param name="nearPlane">Near plane</param>
        /// <param name="farPlane">Far plane</param>
        public OrthographicCamera(float width, float height, float nearPlane, float farPlane)
            : base(Vector3.Zero)
        {
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, width, 0, height, nearPlane, farPlane);
        }
    }
}
