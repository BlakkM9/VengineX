using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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
        public InputManager Input { get; }

        public Canvas Canvas { get; }

        /// <summary>
        /// The element that currently has keyboard focus
        /// </summary>
        public UIElement? FocusedElement { get; private set; } = null;

        /// <summary>
        /// The topmost element the cursor is currently above.
        /// </summary>
        public UIElement? CurrentElement
        {
            get => _currentElement;
            private set
            {
                _previousElement = _currentElement;
                _currentElement = value;
            }
        }
        private UIElement? _currentElement = null;

        /// <summary>
        /// Topmost UIElement the cursor was above before.
        /// </summary>
        private UIElement? _previousElement = null;

        /// <summary>
        /// Creates a new event system with the given input manager.
        /// </summary>
        public UIEventSystem(InputManager input, Canvas canvas)
        {
            Input = input;
            Canvas = canvas;

            // Register listeners to native input events
            input.Window.MouseMove += Window_MouseMove;
            input.Window.MouseDown += Window_MouseDown;
            input.Window.MouseUp += Window_MouseUp;
            input.Window.MouseWheel += Window_MouseWheel;
            input.Window.KeyDown += Window_KeyDown;
            input.Window.KeyUp += Window_KeyUp;
            input.Window.TextInput += Window_TextInput;
        }

        private void Window_MouseMove(MouseMoveEventArgs args)
        {
            Vector2 position = new Vector2(args.X, args.Y);
            
            foreach (UIElement child in Canvas.AllChildren())
            {
                if (child.IgnoreInputEvents) { continue; }

                // Mouse entered / left
                if (child.ContainsAbsolute(position) && !child.MouseOver)
                {
                    child.MouseOver = true;
                    if (Input.MouseState.IsAnyButtonDown) { child.MouseDown = true; }
                    child.InvokeEntered(args);
                }
                else if (!child.ContainsAbsolute(position) && child.MouseOver)
                {
                    child.MouseOver = false;
                    child.MouseDown = false;
                    child.InvokeLeft(args);
                }
            }
        }

        private void Window_MouseDown(MouseButtonEventArgs args)
        {
            CurrentElement = FindTopmostElement(Input.MouseState.Position);

            if (CurrentElement != null)
            {
                if (_previousElement != null)
                {
                    _previousElement.ClickInitiated = false;
                }

                if (FocusedElement != null && FocusedElement.Focused)
                {
                    FocusedElement.InvokeLostFocus();
                    FocusedElement.Focused = false;
                }

                if (CurrentElement.IgnoreInputEvents) { return; }

                CurrentElement.ClickInitiated = true;
                CurrentElement.InvokeMouseButtonPressed(args);
                CurrentElement.MouseDown = true;

                FocusedElement = CurrentElement;
                CurrentElement.Focused = true;
                CurrentElement.InvokeGainedFocus();
            }
            else
            {
                if (_previousElement != null)
                {
                    _previousElement.ClickInitiated = false;
                }

                if (FocusedElement != null && FocusedElement.Focused)
                {
                    FocusedElement.Focused = false;
                    FocusedElement.InvokeLostFocus();
                    FocusedElement = null;
                }
            }
        }


        private void Window_MouseUp(MouseButtonEventArgs args)
        {
            CurrentElement = FindTopmostElement(Input.MouseState.Position);

            if (CurrentElement != null)
            {
                if (CurrentElement.IgnoreInputEvents) { return; }

                CurrentElement.InvokeMouseButtonReleased(args);
                CurrentElement.MouseDown = false;

                if (CurrentElement.ClickInitiated)
                {
                    CurrentElement.InvokeClicked(args);
                }
            }
        }


        private void Window_MouseWheel(MouseWheelEventArgs args)
        {
            CurrentElement = FindTopmostElement(Input.MouseState.Position);
            
            if (CurrentElement != null)
            {
                CurrentElement.InvokeScrolled(args);
            }
        }


        private void Window_TextInput(TextInputEventArgs args)
        {
            FocusedElement?.InvokeTextInput(args);
        }


        private void Window_KeyDown(KeyboardKeyEventArgs args)
        {
            FocusedElement?.InvokeKeyPressed(args);
        }


        private void Window_KeyUp(KeyboardKeyEventArgs args)
        {
            FocusedElement?.InvokeKeyReleased(args);
        }


        /// <summary>
        /// Findes the topmost element at given position that is not ignoring input events.
        /// </summary>
        /// <param name="point">Absolute position to check from.</param>
        /// <returns>null if none found.</returns>
        private UIElement? FindTopmostElement(Vector2 point)
        {
            foreach (UIElement child in Canvas.AllChildren().Reverse())
            {
                if (child.ContainsAbsolute(point) && !child.IgnoreInputEvents)
                {
                    Console.WriteLine(child.GetType());
                    return child;
                }
            }


            return null;
        }
    }
}
