using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <see cref="InputManager"/> providing the input events for the <see cref="UIEventSystem"/>.
        /// </summary>
        public InputManager Input { get; }

        /// <summary>
        /// Canvas holding all the UIElement that should be handled by this event system.
        /// </summary>
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
            input.Window.MouseDown += Window_MouseDown;
            input.Window.MouseUp += Window_MouseUp;
            input.Window.MouseWheel += Window_MouseWheel;
            input.Window.KeyDown += Window_KeyDown;
            input.Window.KeyUp += Window_KeyUp;
            input.Window.TextInput += Window_TextInput;
        }


        /// <summary>
        /// Call this every frame update. <br/>
        /// This will update the elements enter and left events<br/>
        /// way more resource friendly than doing that from <see cref="NativeWindow.MouseMove"/><br/>,
        /// because like this it is at most checked once per frame, otherwise on every mouse movement.
        /// </summary>
        public virtual void UpdateMouseMove()
        {
            if (Input.MouseCatched) { return; }
            MouseMoveEventArgs args = new MouseMoveEventArgs(Input.MouseState.Position, Input.MouseState.Delta);

            Vector2 position = new Vector2(args.X, args.Y);

            foreach (UIElement child in Canvas.EnumerateChildren(true))
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


        protected virtual void Window_MouseDown(MouseButtonEventArgs args)
        {
            if (Input.MouseCatched) { return; }
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


        protected virtual void Window_MouseUp(MouseButtonEventArgs args)
        {
            if (Input.MouseCatched) { return; }
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


        protected virtual void Window_MouseWheel(MouseWheelEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            CurrentElement = FindTopmostElement(Input.MouseState.Position);
            
            if (CurrentElement != null)
            {
                CurrentElement.InvokeScrolled(args);
            }
        }


        protected virtual void Window_TextInput(TextInputEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            FocusedElement?.InvokeTextInput(args);
        }


        protected virtual void Window_KeyDown(KeyboardKeyEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            FocusedElement?.InvokeKeyPressed(args);
        }


        protected virtual void Window_KeyUp(KeyboardKeyEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            FocusedElement?.InvokeKeyReleased(args);
        }


        /// <summary>
        /// Findes the topmost element at given position that is not ignoring input events.
        /// </summary>
        /// <param name="point">Absolute position to check from.</param>
        /// <returns>null if none found.</returns>
        protected virtual UIElement? FindTopmostElement(Vector2 point)
        {
            foreach (UIElement child in Canvas.EnumerateChildren(true).Reverse())
            {
                if (child.ContainsAbsolute(point) && !child.IgnoreInputEvents) { return child; }
            }

            return null;
        }
    }
}
