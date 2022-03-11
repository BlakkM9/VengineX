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
    }
}
