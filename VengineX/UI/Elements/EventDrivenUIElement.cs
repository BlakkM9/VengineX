using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Rendering;
using VengineX.Input;
using VengineX.UI.Canvases;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Base class for all UI elements that are driven by input events.<br/>
    /// Basically it adds MouseEnter,-Left,-Pressed,-Clicked events to <see cref="UIElement"/>
    /// </summary>
    public abstract class EventDrivenUIElement : UIElement
    {
        #region Events

        /// <summary>
        /// Handler for all mouse events.
        /// </summary>
        public delegate void MouseEventHandler(EventDrivenUIElement sender, MouseState currentMouseState);

        /// <summary>
        /// The mouse cursor entered this UI element.
        /// </summary>
        public event MouseEventHandler? MouseEntered;

        /// <summary>
        /// The mouse cursor left this UI element.
        /// </summary>
        public event MouseEventHandler? MouseLeft;

        /// <summary>
        /// Any mousebutton was pressed while above this element.
        /// </summary>
        public event MouseEventHandler? MouseButtonPressed;

        /// <summary>
        /// Any mousebutton was released while above this element.
        /// </summary>
        public event MouseEventHandler? MouseButtonReleased;

        /// <summary>
        /// Occus when this element was clicked with any mouse button.
        /// </summary>
        public event MouseEventHandler? Clicked;

        /// <summary>
        /// Wether or not the mouse cursor is currently over this UI element.
        /// </summary>
        public bool IsMouseOver { get; protected set; }

        /// <summary>
        /// Wether or not any mouse button is down on this element.
        /// </summary>
        public bool IsMouseDown { get; protected set; }


        protected bool _clickStartedInside;

        #endregion


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public EventDrivenUIElement(float x, float y, float width, float height)
            : base(x, y, width, height) { }


        /// <summary>
        /// Updates this ui element (used for creating input events).
        /// </summary>
        public override void Update()
        {
            if (ParentCanvas != null)
            {
                // Update self.
                UpdateMouseLeftEnter(ParentCanvas.Input.MouseState);
                UpdateMousePressedReleased(ParentCanvas.Input);


                // Update children
                foreach (EventDrivenUIElement child in Children)
                {
                    child.Update();
                }
            }
        }


        protected virtual void UpdateMouseLeftEnter(MouseState mouseState)
        {
            // Mouse entered / left
            if (Rect.Contains(mouseState.X, mouseState.Y) && !IsMouseOver)
            {
                IsMouseOver = true;
                MouseEntered?.Invoke(this, mouseState);
            }
            else if (!Rect.Contains(mouseState.X, mouseState.Y) && IsMouseOver)
            {
                IsMouseOver = false;
                IsMouseDown = false;
                MouseLeft?.Invoke(this, mouseState);
            }
        }


        protected virtual void UpdateMousePressedReleased(InputManager input)
        {
            MouseState ms = input.MouseState;

            // Mouse down / up / pressed / released / clicked
            if (IsMouseOver)
            {
                if (input.AnyMouseButtonPressed)
                {
                    _clickStartedInside = true;
                    MouseButtonPressed?.Invoke(this, ms);
                }
                else if (input.AnyMouseButtonReleased)
                {
                    MouseButtonReleased?.Invoke(this, ms);

                    if (_clickStartedInside)
                    {
                        Clicked?.Invoke(this, ms);
                    }
                }

                if (ms.IsAnyButtonDown && !IsMouseDown)
                {
                    IsMouseDown = true;
                }
                else if (!ms.IsAnyButtonDown && IsMouseDown)
                {
                    IsMouseDown = false;
                }
            }
            else
            {
                if (input.AnyMouseButtonReleased)
                {
                    _clickStartedInside = false;
                }
            }
        }
    }
}
