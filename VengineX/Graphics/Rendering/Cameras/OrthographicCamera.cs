using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Cameras
{

    public class OrthographicCamera : Camera
    {

        public OrthographicCamera(float viewportWidth, float viewportHeight, float nearPlane, float farPlane)
            : base(viewportWidth, viewportHeight)
        {

            ProjectionMatrix = Matrix4.CreateOrthographic(viewportWidth, viewportHeight, nearPlane, farPlane);
        }
    }
}
