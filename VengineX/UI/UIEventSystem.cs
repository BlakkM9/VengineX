using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Input;
using VengineX.UI.Elements;

namespace VengineX.UI
{
    /// <summary>
    /// Controls all the ui events within a canvas.
    /// </summary>
    public class UIEventSystem
    {
        /// <summary>
        /// The input manager this event system is attached to.
        /// </summary>
        public InputManager Input { get; }

        /// <summary>
        /// Creates a new event system with the given input manager.
        /// </summary>
        public UIEventSystem(InputManager input)
        {
            Input = input;
        }


        /// <summary>
        /// Call this every frame to update the events of all the UI events in given canvas.
        /// </summary>
        public void UpdateEvents(Canvas canvas)
        {
            KeyboardState kbs = Input.KeyboardState;
            MouseState ms = Input.MouseState;

            foreach (UIElement child in canvas.AllChildren())
            {
                UpdateMouseLeftEnter(child, ms);
                UpdateMousePressedReleased(child, ms);
            }
        }

        private void UpdateMouseLeftEnter(UIElement element, MouseState mouseState)
        {
            // Mouse entered / left
            if (element.Contains(mouseState.Position) && !element.MouseOver)
            {
                element.MouseOver = true;
                element.InvokeEntered();
            }
            else if (!element.Contains(mouseState.Position) && element.MouseOver)
            {
                element.MouseOver = false;
                element.MouseDown = false;
                element.InvokeLeft();
            }
        }

        private void UpdateMousePressedReleased(UIElement element, MouseState mouseState)
        {
            // Mouse down / up / pressed / released / clicked
            if (element.MouseOver)
            {
                if (Input.AnyMouseButtonPressed)
                {
                    element.ClickStartedInside = true;
                    element.InvokeMouseButtonPressed();
                }
                else if (Input.AnyMouseButtonReleased)
                {
                    element.InvokeMouseButtonReleased();

                    if (element.ClickStartedInside)
                    {
                        element.InvokeClicked();
                    }
                }

                if (mouseState.IsAnyButtonDown && !element.MouseDown)
                {
                    element.MouseDown = true;
                }
                else if (!mouseState.IsAnyButtonDown && element.MouseDown)
                {
                    element.MouseDown = false;
                }
            }
            else
            {
                if (Input.AnyMouseButtonReleased)
                {
                    element.ClickStartedInside = false;
                }
            }
        }
    }
}
