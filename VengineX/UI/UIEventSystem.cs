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
        public UIElement? CurrentElement { get; private set; } = null;

        private UIElement? _prevCurrentElement;

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
                if (child.IgnoreEvents) { continue; }

                // Mouse entered / left
                if (child.Contains(position) && !child.MouseOver)
                {
                    child.MouseOver = true;
                    child.InvokeEntered(args);
                }
                else if (!child.Contains(position) && child.MouseOver)
                {
                    child.MouseOver = false;
                    child.MouseDown = false;
                    child.InvokeLeft(args);
                }
            }
        }

        private void Window_MouseDown(MouseButtonEventArgs args)
        {
            CurrentElement = Canvas.FindElement(Input.MouseState.Position);

            if (CurrentElement != null)
            {
                if (_prevCurrentElement != null)
                {
                    _prevCurrentElement.ClickInitiated = false;
                }

                CurrentElement.ClickInitiated = true;
                _prevCurrentElement = CurrentElement;
                CurrentElement.InvokeMouseButtonPressed(args);
                CurrentElement.MouseDown = true;

                if (FocusedElement != null && FocusedElement.Focused == true)
                {
                    FocusedElement.InvokeLostFocus();
                    FocusedElement.Focused = false;
                }

                FocusedElement = CurrentElement;
                CurrentElement.Focused = true;
                CurrentElement.InvokeGainedFocus();
            }
            else
            {
                if (_prevCurrentElement != null)
                {
                    _prevCurrentElement.ClickInitiated = false;
                }

                if (FocusedElement != null && FocusedElement.Focused == true)
                {
                    FocusedElement.Focused = false;
                    FocusedElement.InvokeLostFocus();
                    FocusedElement = null;
                }
            }
        }


        private void Window_MouseUp(MouseButtonEventArgs args)
        {
            CurrentElement = Canvas.FindElement(Input.MouseState.Position);

            if (CurrentElement != null)
            {
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
            CurrentElement = Canvas.FindElement(Input.MouseState.Position);
            CurrentElement?.InvokeScrolled(args);
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
    }
}
