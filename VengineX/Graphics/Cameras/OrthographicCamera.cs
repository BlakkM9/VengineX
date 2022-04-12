using OpenTK.Mathematics;

namespace VengineX.Graphics.Cameras
{
    /// <summary>
    /// Camera with orthographic projection.
    /// </summary>

    public class OrthographicCamera : Camera
    {
        /// <summary>
        /// Width of the camera viewport.
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Height of the camera viewport.
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Wether the origin of the viewport is the center or not.
        /// </summary>
        public bool OffCenter { get; }


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
            Width = width;
            Height = height;
            OffCenter = offCenter;

            if (offCenter)
            {
                ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, width, 0, height, nearPlane, farPlane);
            }
            else
            {
                ProjectionMatrix = Matrix4.CreateOrthographic(width, height, nearPlane, farPlane);
            }
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void SetClippingPlanes(float farPlane, float nearPlane)
        {
            if (OffCenter)
            {
                ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, nearPlane, farPlane);
            }
            else
            {
                ProjectionMatrix = Matrix4.CreateOrthographic(Width, Height, nearPlane, farPlane);
            }
        }
    }
}
