using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Cameras
{
    public class FPSCamera : PerspectiveCamera
    {

        protected static readonly Vector3 xzOne = new Vector3(1.0f, 0.0f, 1.0f);

        public float Sensitivity { get; set; }
        public float Speed { get; set; }

        protected float xRot;
        protected float yRot;

        protected Vector3 front;
        protected Vector3 lookAt;


        public FPSCamera(Vector3 position, float fov, float viewportWidth, float viewportHeight, float nearPlane, float farPlane)
            : base(position, fov, viewportWidth, viewportHeight, nearPlane, farPlane)
        {

            Sensitivity = 0.05f;
            Speed = 6.0f;

            xRot = -90.0f;
            yRot = 0.0f;

            front = new Vector3(0.0f, 0.0f, -1.0f);
            lookAt = Vector3.Normalize(front);
        }


        public virtual void Move(Vector3 direction, double delta)
        {
            MoveFront((float)(direction.X * Speed * delta));
            MoveSide((float)(direction.Z * Speed * delta));
        }

        public virtual void Rotate(Vector2 relativeRotation)
        {
            xRot += relativeRotation.X * Sensitivity;
            yRot += -relativeRotation.Y * Sensitivity;

            yRot = MathHelper.Clamp(yRot, -89.0f, 89.0f);

            float xRotRad = MathHelper.DegreesToRadians(xRot);
            float yRotRad = MathHelper.DegreesToRadians(yRot);

            front.X = (float)(MathHelper.Cos(yRotRad) * MathHelper.Cos(xRotRad));
            front.Y = (float)MathHelper.Sin(yRotRad);
            front.Z = (float)(MathHelper.Cos(yRotRad) * MathHelper.Sin(xRotRad));

            lookAt = Vector3.Normalize(front);
        }


        /// <summary>
        /// Sets the rotation of the camera
        /// </summary>
        /// <param name="rotation">Rotation in degrees for x and y direction</param>
        public virtual void SetRotation(Vector2 rotation)
        {
            xRot = rotation.X;
            yRot = rotation.Y;

            float xRotRad = MathHelper.DegreesToRadians(xRot);
            float yRotRad = MathHelper.DegreesToRadians(yRot);

            front.X = (float)(MathHelper.Cos(yRotRad) * MathHelper.Cos(xRotRad));
            front.Y = (float)MathHelper.Sin(yRotRad);
            front.Z = (float)(MathHelper.Cos(yRotRad) * MathHelper.Sin(xRotRad));

            lookAt = Vector3.Normalize(front);
        }


        public override void Update()
        {
            ViewMatrix = Matrix4.LookAt(Position, Position + lookAt, Vector3.UnitY);
            Frustum.CalculateFrustum(ProjectionMatrix, ViewMatrix);
        }


        protected virtual void MoveFront(float amount)
        {
            Translate(Vector3.Normalize(xzOne * lookAt) * amount);
        }


        protected virtual void MoveSide(float amount)
        {
            Translate(Vector3.Normalize(Vector3.Cross(lookAt, Vector3.UnitY)) * amount);
        }
    }
}
