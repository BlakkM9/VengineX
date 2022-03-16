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
using VengineX.UI.Canvases;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Base class for all UI elements.
    /// </summary>
    public abstract class UIElement : IRenderable
    {
        #region Input

        /// <summary>
        /// Handler for all mouse events.
        /// </summary>
        public delegate void MouseEventHandler(UIElement sender, MouseState currentMouseState);

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
        /// Wether or not the mouse cursor is currently over this UI element.
        /// </summary>
        public bool IsMouseOver { get; protected set; }

        /// <summary>
        /// Wether or not any mouse button is down on this element.
        /// </summary>
        public bool IsMouseDown { get; protected set; }

        #endregion

        /// <summary>
        /// The parent element of this UI element.<br/>
        /// If no parent it is null.
        /// </summary>
        public UIElement? ParentElement { get; protected set; }

        /// <summary>
        /// The root canvas for this UI element.<br/>
        /// null means this element is not in any canvas.
        /// </summary>
        public Canvas? ParentCanvas { get; protected set; }

        /// <summary>
        /// The absolute x position in the canvas.
        /// </summary>
        public virtual float X
        {
            get => Rect.X;
            set => Rect = new RectangleF(value, Y, Width, Height);
        }

        /// <summary>
        /// The absolute y position in the canvas.
        /// </summary>
        public virtual float Y
        {
            get => Rect.Y;
            set => Rect = new RectangleF(X, value, Width, Height);
        }

        /// <summary>
        /// The width of this ui element.
        /// </summary>
        public virtual float Width
        {
            get => Rect.Width;
            set => Rect = new RectangleF(X, Y, value, Height);
        }

        /// <summary>
        /// The height of this ui element.
        /// </summary>
        public virtual float Height
        {
            get => Rect.Height;
            set => Rect = new RectangleF(X, Y, Width, value);
        }

        /// <summary>
        /// The internal rect, used for all the mouse events.
        /// </summary>
        public RectangleF Rect
        {
            get => _rect;
            set
            { 
                _rect = value;
                CalculateModelMatrix();
            }
        }
        private RectangleF _rect;

        /// <summary>
        /// ModelMatrix used for transforming the Quad the UIElement is rendered on.
        /// </summary>
        public ref Matrix4 ModelMatrix { get => ref _modelMatrix; }
        private Matrix4 _modelMatrix;

        public HorizontalOrientation HorizontalOrientation { get; set; }

        public VerticalOrientation VerticalOrientation { get; set; }

        /// <summary>
        /// All children that are in this UI element.
        /// </summary>
        protected List<UIElement> Children { get; set; }


        /// <summary>
        /// Creates a new UIElement.<br/>
        /// All the parameters are based of the left upper edge.<br/>
        /// Canvas 0, 0 is left upper edge aswell.
        /// </summary>
        /// <param name="x">X position of the element.</param>
        /// <param name="y">Y position of the element.</param>
        /// <param name="width">Width of the element.</param>
        /// <param name="height">Height of the element.</param>
        public UIElement(float x, float y, float width, float height)
        {
            Rect = new RectangleF(x, y, width, height);
            Children = new List<UIElement>();
        }


        /// <summary>
        /// Updates this ui element (used for creating input events).
        /// </summary>
        public virtual void Update()
        {
            if (ParentCanvas != null)
            {
                // Update self.
                MouseState ms = ParentCanvas.Input.MouseState;

                // Mouse entered / left
                if (Rect.Contains(ms.X, ms.Y) && !IsMouseOver)
                {
                    IsMouseOver = true;
                    MouseEntered?.Invoke(this, ms);
                }
                else if (!Rect.Contains(ms.X, ms.Y) && IsMouseOver)
                {
                    IsMouseOver = false;
                    MouseLeft?.Invoke(this, ms);
                }

                // Mouse down / up
                if (IsMouseOver)
                {


                    if (ms.IsAnyButtonDown && !IsMouseDown)
                    {
                        Console.WriteLine("MB Pressed");
                        IsMouseDown = true;
                        MouseButtonPressed?.Invoke(this, ms);
                    }
                    else if (!ms.IsAnyButtonDown && IsMouseDown)
                    {
                        Console.WriteLine("MB Released");
                        IsMouseDown = false;
                        MouseButtonReleased?.Invoke(this, ms);
                    }
                }

                // Update children
                foreach (UIElement child in Children)
                {
                    child.Update();
                }
            }
        }


        /// <summary>
        /// Calculates the ModelMatrix based on width, height, x, and y.<br/>
        /// Called when automatically when any of these properties change.
        /// </summary>
        protected abstract void CalculateModelMatrix();


        /// <summary>
        /// Calculates and updates the layout (positiona and size) based on the current<br/>
        /// <see cref="HorizontalOrientation"/> and <see cref="VerticalOrientation"/>.
        /// </summary>
        public virtual void UpdateLayout()
        {
            float x = 0;
            float y = 0;
            float w = Width;
            float h = Height;

            // Horizontal
            switch (HorizontalOrientation)
            {
                case HorizontalOrientation.Unset:
                    x = X;
                    break;
                case HorizontalOrientation.Left:
                    break;
                case HorizontalOrientation.Right:
                    x = ParentElement.Width - Width;
                    break;
                case HorizontalOrientation.Center:
                    x = (ParentElement.Width - Width) / 2;
                    break;
                case HorizontalOrientation.Stretch:
                    w = ParentElement.Width;
                    break;
            }


            // Vertical
            switch (VerticalOrientation)
            {
                case VerticalOrientation.Unset:
                    y = Y;
                    break;
                case VerticalOrientation.Top:
                    break;
                case VerticalOrientation.Bottom:
                    y = ParentElement.Height - Height;
                    break;
                case VerticalOrientation.Center:
                    y = (ParentElement.Height - Height) / 2;
                    break;
                case VerticalOrientation.Stretch:
                    h = ParentElement.Height;
                    break;
            }


            Rect = new RectangleF(x, y, w, h);
        }


        /// <summary>
        /// Renders this ui element.
        /// </summary>
        public abstract void Render();


        /// <summary>
        /// Adds given UI element as child of this element.<br/>
        /// Does not change the layout(position/size) of the child to be added.
        /// </summary>
        public virtual void AddChild(UIElement element) => AddChild(element, HorizontalOrientation.Unset, VerticalOrientation.Unset);


        /// <summary>
        /// Adds the given UI element as a child of this element<br/>
        /// with given layout.
        /// </summary>
        public virtual void AddChild(
            UIElement element,
            HorizontalOrientation horizontalOrientation,
            VerticalOrientation verticalOrientation)
        {
            Children.Add(element);
            element.HorizontalOrientation = horizontalOrientation;
            element.VerticalOrientation = verticalOrientation;
            element.ParentCanvas = ParentCanvas;
            element.ParentElement = this;

            element.UpdateLayout();
        }


        /// <summary>
        /// Removes all the children (recursively) from this UI Element.<br/>
        /// </summary>
        /// <param name="dispose">Wether or not dispose childs if implementing IDisposable</param>
        public virtual void ClearChildren(bool dispose)
        {
            // ClearChildren for each child
            foreach (UIElement child in Children)
            {
                child.ClearChildren(dispose);
            }
            
            // Dispose direct childs
            if (dispose)
            {
                foreach (UIElement child in Children)
                {
                    (child as IDisposable)?.Dispose();
                }
            }

            // Clear children list
            Children.Clear();
        }
    }
}
