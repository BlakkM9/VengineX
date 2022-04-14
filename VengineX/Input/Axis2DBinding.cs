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
    /// Similiar to <see cref="Axis1DBinding"/> but working on two axis instead.
    /// </summary>
    public class Axis2DBinding : Binding<Vector2>
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
        /// Creates a new 2 dimensional axis binding with given keys.
        /// </summary>
        public Axis2DBinding(Keys keyNegX, Keys keyPosX, Keys keyNegY, Keys keyPosY)
        {
            KeyNegX = keyNegX;
            KeyPosX = keyPosX;
            KeyNegY = keyNegY;
            KeyPosY = keyPosY;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateValue(InputManager input)
        {
            KeyboardState kbs = input.KeyboardState;

            Vector2 newValue = Vector2.Zero;

            if (kbs.IsKeyDown(KeyNegX)) { newValue.X -= 1; }
            if (kbs.IsKeyDown(KeyPosX)) { newValue.X += 1; }

            if (kbs.IsKeyDown(KeyNegY)) { newValue.Y -= 1; }
            if (kbs.IsKeyDown(KeyPosY)) { newValue.Y += 1; }

            Value = newValue;
        }
    }
}
