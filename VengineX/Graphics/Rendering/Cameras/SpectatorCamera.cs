using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Cameras
{
    public class SpectatorCamera : FPSCamera
    {
        public SpectatorCamera(Vector3 position, float fov, float viewportWidth, float viewportHeight, float nearPlane, float farPlane)
            : base(position, fov, viewportWidth, viewportHeight, nearPlane, farPlane) { }

        public override void Move(Vector3 direction, double delta)
        {
            base.Move(direction, delta);
            MoveUp((float)(direction.Y * Speed * delta));
        }

        protected virtual void MoveUp(float amount)
        {
            Translate(Vector3.UnitY * amount);
        }
    }
}
