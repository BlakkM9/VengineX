using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Input
{
    /// <summary>
    /// Interface for input bindings.
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        /// Updates this input binding, with given <see cref="InputManager"/>.
        /// </summary>
        void UpdateValue(InputManager input);
    }
}
