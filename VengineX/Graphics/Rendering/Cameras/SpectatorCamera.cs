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
        public SpectatorCamera(float fov, float viewportWidth, float viewportHeight, float nearPlane, float farPlane)
            : base(fov, viewportWidth, viewportHeight, nearPlane, farPlane) { }

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
