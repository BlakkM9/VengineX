using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements
{
    /// <summary>
    /// Abstract base class for all ui elements.<br/>
    /// Might also be used as a panel that controls the layout of its children.
    /// </summary>
    public abstract class UIElement : IRenderable
    {
        /// <summary>
        /// ModelMatrix of this element, used for transforming the Quad this UIElement is rendered on.
        /// </summary>
        public ref Matrix4 ModelMatrix => ref _modelMatrix;
        private Matrix4 _modelMatrix;

        /// <summary>
        /// The parent canvas of this ui element.
        /// </summary>
        public Canvas ParentCanvas { get; }

        /// <summary>
        /// TParent of this element.
        /// </summary>
        public UIElement? Parent { get; set; } = null;

        /// <summary>
        /// Layout generator.<br/>
        /// This controls how the child elements in this element<br/>
        /// are layouted. To apply changes, call <see cref="UpdateLayout"/>
        /// </summary>
        public Layout? Layout { get; set; } = null;

        /// <summary>
        /// Position relative to parent.
        /// </summary>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// Absolute position of the ui element (on the ui canvas).
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get => Parent == null ? Position : Parent.AbsolutePosition + Position;
        }

        /// <summary>
        /// Size of this element.
        /// </summary>
        public Vector2 Size { get; set; } = Vector2.Zero;

        /// <summary>
        /// Width of this element. Shortcut for <see cref="Size"/>.
        /// </summary>
        public float Width
        {
            get => Size.X;
            set => Size = new Vector2(value, Height);
        }

        /// <summary>
        /// Height of this element. Shortcut for <see cref="Size"/>.
        /// </summary>
        public float Height
        {
            get => Size.Y;
            set => Size = new Vector2(Width, value);
        }

        /// <summary>
        /// Wether or not the element is currently visible (assuming all parents are visible).
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Check if this element is currently visible, taking parent elements into account.
        /// </summary>
        public bool VisibleRecursive
        {
            get
            {
                bool visible = true;
                UIElement? element = this;
                while (element != null)
                {
                    visible = element.Visible;
                    element = element.Parent;
                }

                return visible;
            }
        }

        /// <summary>
        /// Number of child elements.
        /// </summary>
        public int ChildCount { get => Children.Count; }

        /// <summary>
        /// List holding all the children of this ui element.
        /// </summary>
        public List<UIElement> Children { get; }

        //public bool Enabled { get; set; } = true;

        //public bool IgnoreLayout { get; set; } = true;

        /// <summary>
        /// Calculates the preferred size of this element.<br/>
        /// Preferred size is the size of this element needed to fit all its layouted children.
        /// </summary>
        public Vector2 PreferredSize { get => Layout == null ? Size : Layout.PreferredSize(this); }


        /// <summary>
        /// Creates a new ui element with the given element as parent.
        /// </summary>
        public UIElement(UIElement? parent)
        {
            Children = new List<UIElement>();

            if (parent != null)
            {
                parent.AddChild(this);
                ParentCanvas = parent.ParentCanvas;
            }
            else
            {
                if (GetType() == typeof(Canvas))
                {
                    ParentCanvas = (Canvas)this;
                }
                else
                {
                    throw new ArgumentException("Parent can not be null, except for Canvas!");
                }
            }
        }

        /// <summary>
        /// Iterates over the tree of children.
        /// </summary>
        public IEnumerable<UIElement> AllChildren()
        {
            foreach (UIElement child in Children)
            {
                yield return child;

                foreach (UIElement c in child.AllChildren())
                {
                    yield return c;
                }
            }
        }


        /// <summary>
        /// Adds child element at given index.
        /// </summary>
        public virtual void AddChild(int index, UIElement element)
        {
            Children.Insert(index, element);
            element.Parent = this;
        }


        /// <summary>
        /// Adds child at the end.
        /// </summary>
        public void AddChild(UIElement element)
        {
            Children.Add(element);
            element.Parent = this;
        }


        /// <summary>
        /// Removes child at given index.
        /// </summary>
        public void RemoveChild(int index)
        {
            Children.RemoveAt(index);
        }


        /// <summary>
        /// Removes given child.
        /// </summary>
        public void RemoveChild(UIElement element) => Children.Remove(element);


        /// <summary>
        /// Returns the index of given child.
        /// </summary>
        public void IndexOf(UIElement element) => Children.IndexOf(element);


        /// <summary>
        /// Check if this element contains given point.
        /// </summary>
        public bool Contains(Vector2 point)
        {
            Vector2 distance = point - AbsolutePosition;
            return distance.X >= 0 && distance.Y >= 0 && distance.X < Size.X && distance.Y < Size.Y;
        }


        /// <summary>
        /// Finds and returns the element at the given position (recursive).<br/>
        /// Null if didn't find any.
        /// </summary>
        public UIElement? FindElement(Vector2 point)
        {
            foreach (UIElement child in Children)
            {
                if (child.Visible && child.Contains(point - Position))
                {
                    return child.FindElement(point - Position);
                }
            }
            return Contains(point) && GetType() != typeof(Canvas) ? this : null;
        }


        /// <summary>
        /// Performs layouting for this element and updates the model matrix (recursive).
        /// </summary>
        public virtual void UpdateLayout()
        {
            if (Layout != null)
            {
                Layout.UpdateLayout(this);
            }

            foreach (UIElement child in Children)
            {
                child.UpdateLayout();
            }

            CalculateModelMatrix();
        }


        /// <summary>
        /// Renders this element (and all children)
        /// </summary>
        public virtual void Render()
        {
            if (Children.Count == 0) { return; }

            foreach (UIElement child in Children)
            {
                if (child.Visible)
                {
                    child.Render();
                }
            }
        }


        /// <summary>
        /// Calculates the model matrix of this element, based on <see cref="Size"/> and <see cref="AbsolutePosition"/><br/>
        /// </summary>
        protected virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= Matrix4.CreateScale(Width / 2f, Height / 2f, 0);
            ModelMatrix *= Matrix4.CreateTranslation(Width / 2f + AbsolutePosition.X, -(Height / 2f + AbsolutePosition.Y), 0);
        }


        #region Events

        /// <summary>
        /// Handler for all mouse events.
        /// </summary>
        public delegate void MouseEventHandler(UIElement sender);

        /// <summary>
        /// The mouse cursor entered this UI element.
        /// </summary>
        public event MouseEventHandler? Entered;

        /// <summary>
        /// The mouse cursor left this UI element.
        /// </summary>
        public event MouseEventHandler? Left;

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
        public bool MouseOver { get; internal set; }

        /// <summary>
        /// Wether or not any mouse button is down on this element.
        /// </summary>
        public bool MouseDown { get; internal set; }


        internal bool ClickStartedInside { get; set; }



        internal void InvokeEntered() => Entered?.Invoke(this);

        internal void InvokeLeft() => Left?.Invoke(this);

        internal void InvokeMouseButtonPressed() => MouseButtonPressed?.Invoke(this);

        internal void InvokeMouseButtonReleased() => MouseButtonReleased?.Invoke(this);

        internal void InvokeClicked() => Clicked?.Invoke(this);

        #endregion
    }
}
