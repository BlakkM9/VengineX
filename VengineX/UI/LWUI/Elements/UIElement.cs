using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.UI.LWUI.Layouts;

namespace VengineX.UI.LWUI.Elements
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

        public Canvas ParentCanvas { get; }

        /// <summary>
        /// TParent of this element.
        /// </summary>
        public UIElement? Parent { get; set; } = null;

        /// <summary>
        /// Layout generator of this element.
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
        public float Width { get => Size.X; }

        /// <summary>
        /// Height of this element. Shortcut for <see cref="Size"/>.
        /// </summary>
        public float Height { get => Size.Y; }

        /// <summary>
        /// If nonzero, components of the fixed size attribute override any values
        /// computed by a layout generator associated with this widget. Note that
        /// just setting the fixed size alone is not enough to actually change its
        /// size; this is done with a call to \ref setSize or a call to \ref performLayout()
        /// in the parent widget.
        /// </summary>
        public Vector2 FixedSize { get; set; } = Vector2.Zero;

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


        public List<UIElement> Children { get; }


        public bool Enabled { get; set; } = true;

        public bool Focuesd { get; set; } = false;

        /// <summary>
        /// Calculates the preferred size of this element.
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
        public void RemoveChild(int index) => Children.RemoveAt(index);


        /// <summary>
        /// Removes given child.
        /// </summary>
        public void RemoveChild(UIElement element) => Children.Remove(element);


        /// <summary>
        /// Returns the index of given child.
        /// </summary>
        public void IndexOf(UIElement element) => Children.IndexOf(element);


        public void RequestFocus()
        {
            UIElement element = this;
            while (element.Parent != null)
            {
                element = element.Parent;
            }

            throw new NotImplementedException();
        }


        /// <summary>
        /// Check if this element contains given point.
        /// </summary>
        public bool Contains(Vector2 point)
        {
            Vector2 distance = point - Position;
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
            return Contains(point) ? this : null;
        }


        /// <summary>
        /// Performs layouting for this element (recursive).
        /// </summary>
        public virtual void PerformLayout()
        {
            if (Layout != null)
            {
                Layout.PerformLayout(this);
            }
            else
            {
                foreach (UIElement child in Children)
                {
                    Vector2 preferredSize = child.PreferredSize;
                    Vector2 fixedSize = child.FixedSize;
                    child.Size = new Vector2(
                        fixedSize[0] != 0 ? fixedSize[0] : preferredSize[0],
                        fixedSize[1] != 0 ? fixedSize[1] : preferredSize[1]);
                    child.PerformLayout();
                }
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
        public virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= Matrix4.CreateScale(Width / 2f, Height / 2f, 0);
            ModelMatrix *= Matrix4.CreateTranslation(Width / 2f + AbsolutePosition.X, -(Height / 2f + AbsolutePosition.Y), 0);
        }
    }
}
