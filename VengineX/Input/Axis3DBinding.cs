using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Input
{
    /// <summary>
    /// Similiar to <see cref="Axis1DBinding"/> but working on three axis instead.
    /// </summary>
    public class Axis3DBinding : Binding<Vector3>
    {
        /// <summary>
        /// Key that changes the x value to -1.
        /// </summary>
        public Keys KeyNegX { get; set; }

        /// <summary>
        /// Key that changes the x value to +1.
        /// </summary>
        public Keys KeyPosX { get; set; }

        /// <summary>
        /// Key that changes the y value to -1.
        /// </summary>
        public Keys KeyNegY { get; set; }

        /// <summary>
        /// Key that changes the y value to +1.
        /// </summary>
        public Keys KeyPosY { get; set; }

        /// <summary>
        /// Key that changes the z value to -1.
        /// </summary>
        public Keys KeyNegZ { get; set; }

        /// <summary>
        /// Key that changes the z value to +1.
        /// </summary>
        public Keys KeyPosZ { get; set; }


        /// <summary>
        /// Creates a new 3 dimensional axis binding with for given keys.
        /// </summary>
        public Axis3DBinding(Keys keyNegX, Keys keyPosX, Keys keyNegY, Keys keyPosY, Keys keyNegZ, Keys keyPosZ)
        {
            KeyNegX = keyNegX;
            KeyPosX = keyPosX;
            KeyNegY = keyNegY;
            KeyPosY = keyPosY;
            KeyNegZ = keyNegZ;
            KeyPosZ = keyPosZ;
        }

        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateValue(InputManager input)
        {
            KeyboardState kbs = input.KeyboardState;

            Vector3 newValue = Vector3.Zero;

            if (kbs.IsKeyDown(KeyNegX)) { newValue.X -= 1; }
            if (kbs.IsKeyDown(KeyPosX)) { newValue.X += 1; }

            if (kbs.IsKeyDown(KeyNegY)) { newValue.Y -= 1; }
            if (kbs.IsKeyDown(KeyPosY)) { newValue.Y += 1; }

            if (kbs.IsKeyDown(KeyNegZ)) { newValue.Z -= 1; }
            if (kbs.IsKeyDown(KeyPosZ)) { newValue.Z += 1; }

            Value = newValue;
        }
    }
}
