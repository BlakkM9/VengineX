using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Input
{
    /// <summary>
    /// Abstract base class for input bindings.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Value"/> for the binding.</typeparam>
    public abstract class Binding<T> : IBinding where T : struct
    {
        /// <summary>
        /// Event handler for <see cref="ValueChanged"/>.
        /// </summary>
        /// <param name="binding">The binding that triggered the event.</param>
        /// <param name="value">The new value.</param>
        public delegate void ValueChangedHandler(Binding<T> binding, T value);

        /// <summary>
        /// This event occurs when the value of this binding changed.
        /// </summary>
        public event ValueChangedHandler ValueChanged;

        /// <summary>
        /// The current value of this binding in the current update frame.
        /// </summary>
        public T Value
        {
            get => _value;
            protected set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    ValueChanged?.Invoke(this, _value);
                }
            }
        }
        private T _value;


        /// <summary>
        /// Creates a new binding and sets the value to default.
        /// </summary>
        public Binding()
        {
            _value = default;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void UpdateValue(InputManager input);
    }
}
