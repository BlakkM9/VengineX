using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;

namespace VengineX.Input
{
    /// <summary>
    /// A binding of a single key action that is either true or false.
    /// </summary>
    public class ActionBinding : Binding<bool>
    {
        /// <summary>
        /// The key that triggers this binding.
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// The type of the action. see <see cref="KeyActionType"/> for more details.
        /// </summary>
        public KeyActionType ActionType { get; set; }

        /// <summary>
        /// Time in seconds that passed since the key was last pressed (only used for <see cref="KeyActionType.DoublePress"/>)
        /// </summary>
        private double _timeSinceLastPress;

        
        /// <summary>
        /// Creates a new action binding for given key and action type.
        /// </summary>
        public ActionBinding(Keys key, KeyActionType actionType)
        {
            Key = key;
            ActionType = actionType;
            _timeSinceLastPress = double.MaxValue;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void UpdateValue(InputManager input)
        {
            KeyboardState kbs = input.KeyboardState;

            switch (ActionType)
            {
                case KeyActionType.Hold:
                    Value = kbs.IsKeyDown(Key);
                    return;
                case KeyActionType.Release:
                    Value = kbs.IsKeyReleased(Key);
                    break;
                case KeyActionType.Press:
                    Value = kbs.IsKeyPressed(Key);
                    break;
                case KeyActionType.DoublePress:
                    if (kbs.IsKeyPressed(Key))
                    {
                        Value = _timeSinceLastPress <= input.MaxDoublePressTimeframe;
                        _timeSinceLastPress = 0;
                    }
                    else
                    {
                        Value = false;
                        _timeSinceLastPress += Time.DeltaUpdate;
                    }
                    break;
            }
        }
    }
}
