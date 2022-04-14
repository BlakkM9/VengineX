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
    /// Binding for a one dimensional axis between two key.<br/>
    /// The value ranges from -1 to 1 and is determined by<br/>
    /// <see cref="KeyNeg"/> and <see cref="KeyPos"/> being down.<br/>
    /// If both keys are down, the value will be 0.
    /// </summary>
    public class Axis1DBinding : Binding<float>
    {
        /// <summary>
        /// Key that changes the value to -1.
        /// </summary>
        public Keys KeyNeg { get; set; }

        /// <summary>
        /// Key that changes the value to +1.
        /// </summary>
        public Keys KeyPos { get; set; }


        /// <summary>
        /// Creates a new 1 dimensional axis binding with given keys.
        /// </summary>
        public Axis1DBinding(Keys keyNeg, Keys keyPos)
        {
            KeyNeg = keyNeg;
            KeyPos = keyPos;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateValue(InputManager input)
        {
            KeyboardState kbs = input.KeyboardState;

            float newValue = 0;

            if (kbs.IsKeyDown(KeyNeg)) { newValue -= 1; }
            if (kbs.IsKeyDown(KeyPos)) { newValue += 1; }

            Value = newValue;
        }
    }
}
