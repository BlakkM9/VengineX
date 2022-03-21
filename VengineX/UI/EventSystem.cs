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
    public class EventSystem
    {
        /// <summary>
        /// <see cref="InputManager"/> providing the input events for the <see cref="EventSystem"/>.
        /// </summary>
        public InputManager Input { get; }

        /// <summary>
        /// Canvas holding all the UIElement that should be handled by this event system.
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// The element that currently has keyboard focus
        /// </summary>
        public Element? FocusedElement { get; private set; } = null;

        /// <summary>
        /// The topmost element the cursor is currently above.
        /// </summary>
        public Element? CurrentElement
        {
            get => _currentElement;
            private set
            {
                _previousElement = _currentElement;
                _currentElement = value;
            }
        }
        private Element? _currentElement = null;

        /// <summary>
        /// Topmost UIElement the cursor was above before.
        /// </summary>
        private Element? _previousElement = null;

        /// <summary>
        /// Creates a new event system with the given input manager.
        /// </summary>
        public EventSystem(InputManager input, Canvas canvas)
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

            foreach (Element child in Canvas.EnumerateChildren(true))
            {
                if (child.IgnoreInputEvents) { continue; }

                // Mouse entered / left
                if (child.ContainsAbsolute(position) && !child.Events.MouseOver)
                {
                    child.Events.MouseOver = true;
                    if (Input.MouseState.IsAnyButtonDown) { child.Events.MouseDown = true; }
                    child.Events.InvokeEntered(args);
                }
                else if (!child.ContainsAbsolute(position) && child.Events.MouseOver)
                {
                    child.Events.MouseOver = false;
                    child.Events.MouseDown = false;
                    child.Events.InvokeLeft(args);
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
                    _previousElement.Events.ClickInitiated = false;
                }

                if (FocusedElement != null && FocusedElement.Events.Focused)
                {
                    FocusedElement.Events.InvokeLostFocus();
                    FocusedElement.Events.Focused = false;
                }

                if (CurrentElement.IgnoreInputEvents) { return; }

                CurrentElement.Events.ClickInitiated = true;
                CurrentElement.Events.InvokeMouseButtonPressed(args);
                CurrentElement.Events.MouseDown = true;

                FocusedElement = CurrentElement;
                CurrentElement.Events.Focused = true;
                CurrentElement.Events.InvokeGainedFocus();
            }
            else
            {
                if (_previousElement != null)
                {
                    _previousElement.Events.ClickInitiated = false;
                }

                if (FocusedElement != null && FocusedElement.Events.Focused)
                {
                    FocusedElement.Events.Focused = false;
                    FocusedElement.Events.InvokeLostFocus();
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

                CurrentElement.Events.InvokeMouseButtonReleased(args);
                CurrentElement.Events.MouseDown = false;

                if (CurrentElement.Events.ClickInitiated)
                {
                    CurrentElement.Events.InvokeClicked(args);
                }
            }
        }


        protected virtual void Window_MouseWheel(MouseWheelEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            CurrentElement = FindTopmostElement(Input.MouseState.Position);
            
            if (CurrentElement != null)
            {
                CurrentElement.Events.InvokeScrolled(args);
            }
        }


        protected virtual void Window_TextInput(TextInputEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            FocusedElement?.Events.InvokeTextInput(args);
        }


        protected virtual void Window_KeyDown(KeyboardKeyEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            FocusedElement?.Events.InvokeKeyPressed(args);
        }


        protected virtual void Window_KeyUp(KeyboardKeyEventArgs args)
        {
            if (Input.MouseCatched) { return; }
            FocusedElement?.Events.InvokeKeyReleased(args);
        }


        /// <summary>
        /// Findes the topmost element at given position that is not ignoring input events.
        /// </summary>
        /// <param name="point">Absolute position to check from.</param>
        /// <returns>null if none found.</returns>
        protected virtual Element? FindTopmostElement(Vector2 point)
        {
            foreach (Element child in Canvas.EnumerateChildren(true).Reverse())
            {
                if (child.ContainsAbsolute(point) && !child.IgnoreInputEvents) { return child; }
            }

            return null;
        }
    }
}
