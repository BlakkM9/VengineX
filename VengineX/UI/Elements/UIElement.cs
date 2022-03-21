using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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
        public UIElement Parent { get; set; } = null;

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
        /// X positon relative to its parent. Shortcut for <see cref="Position"/>
        /// </summary>
        public float X {
            get => Position.X;
            set => Position = new Vector2(value, Position.Y);
        }

        /// <summary>
        /// X positon relative to its parent. Shortcut for <see cref="Position"/>
        /// </summary>
        public float Y
        {
            get => Position.Y;
            set => Position = new Vector2(Position.Y, value);
        }

        /// <summary>
        /// Margin of this element. Order is Left, Top, Right, Down.<br/>
        /// Margin is used to offset this elements position and size when layouting.<br/>
        /// If this element is <see cref="IgnoreLayout"/> margin is ignored aswell.<br/>
        /// </summary>
        public Vector4 Margin { get; set; } = Vector4.Zero;

        /// <summary>
        /// Left margin. Shortcut for <see cref="Margin"/>.
        /// </summary>
        public float MarginLeft
        {
            get => Margin.X;
            set => Margin = new Vector4(value, Margin.Y, Margin.Z, Margin.W);
        }

        /// <summary>
        /// Top margin. Shortcut for <see cref="Margin"/>.
        /// </summary>
        public float MarginTop
        {
            get => Margin.Y;
            set => Margin = new Vector4(Margin.X, value, Margin.Z, Margin.W);
        }

        /// <summary>
        /// Right margin. Shortcut for <see cref="Margin"/>.
        /// </summary>
        public float MarginRight
        {
            get => Margin.Z;
            set => Margin = new Vector4(Margin.X, Margin.Y, value, Margin.W);
        }

        /// <summary>
        /// Bottom margin. Shortcut for <see cref="Margin"/>.
        /// </summary>
        public float MarginBottom
        {
            get => Margin.W;
            set => Margin = new Vector4(Margin.X, Margin.Y, Margin.Z, value);
        }

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
        /// Size of this element including margin.
        /// </summary>
        public Vector2 TotalSize => Size + new Vector2(MarginLeft + MarginRight, MarginTop + MarginBottom);

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
        /// Width of this element including margin.
        /// </summary>
        public float TotalWidth => Width + MarginLeft + MarginRight;

        /// <summary>
        /// Width of this element including the margin.
        /// </summary>
        public float TotalHeight => Height + MarginTop + MarginBottom;

        /// <summary>
        /// If set to true, this element will ignore all layouting.<br/>
        /// Children of this element will still be layouted (if their <see cref="IgnoreLayout"/> is true).
        /// </summary>
        public bool IgnoreLayout { get; set; } = false;

        /// <summary>
        /// Wether or not the element is currently visible
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Sets the visibility of self and all children.<br/>
        /// </summary>
        public bool VisibleRecursive
        {
            set
            {
                foreach (UIElement child in Children)
                {
                    child.VisibleRecursive = value;
                }
            }
        }

        /// <summary>
        /// Number of child elements.
        /// </summary>
        public int ChildCount => Children.Count;

        /// <summary>
        /// List holding all the children of this ui element.
        /// </summary>
        public List<UIElement> Children { get; }

        /// <summary>
        /// Calculates the preferred size of this element.<br/>
        /// Preferred size is the size of this element needed to fit all its layouted children.
        /// </summary>
        public Vector2 PreferredSize => Layout == null ? Size : Layout.PreferredSize(this);


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

                foreach (UIElement childsChild in child.AllChildren())
                {
                    yield return childsChild;
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
        /// Check if this element contains given point in absolute (relative to canvas) space.
        /// </summary>
        public bool ContainsAbsolute(Vector2 point)
        {
            Vector2 distance = point - AbsolutePosition;
            return distance.X >= 0 && distance.Y >= 0 && distance.X < Size.X && distance.Y < Size.Y;
        }

        /// <summary>
        /// Check if this element contains given point in relative (to parent) space.
        /// </summary>
        public bool ContainsRelative(Vector2 point)
        {
            Vector2 distance = point - Position;
            return distance.X >= 0 && distance.Y >= 0 && distance.X < Size.X && distance.Y < Size.Y;
        }


        /// <summary>
        /// Updates the layouting for this element and updates the model matrix (recursive).
        /// </summary>
        public virtual void UpdateLayout()
        {
            // Update all children first (so their size is updated for own layouting)
            foreach (UIElement child in Children)
            {
                child.UpdateLayout();
            }

            // Update own layout (layouts all direct children)
            Layout?.UpdateLayout(this);
        }


        /// <summary>
        /// Renders this element (and all children)
        /// </summary>
        public virtual void Render()
        {
            if (Children.Count == 0) { return; }

            foreach (UIElement child in Children)
            {
                child.Render();
            }
        }


        /// <summary>
        /// Calculates the model matrix of this element, based on <see cref="Size"/> and <see cref="AbsolutePosition"/><br/>
        /// </summary>
        protected virtual void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Width / 2f, Height / 2f, 0);
            ModelMatrix *= Matrix4.CreateTranslation(Width / 2f + AbsolutePosition.X, -(Height / 2f + AbsolutePosition.Y), 0);
        }


        #region Events

        /// <summary>
        /// If this is set to true, this element will not receive ui events.
        /// </summary>
        public bool IgnoreInputEvents { get; set; } = false;

        /// <summary>
        /// Handler for generic ui events.
        /// </summary>
        public delegate void UIEventHandler(UIElement sender);

        /// <summary>
        /// Handler mouse move events.
        /// </summary>
        public delegate void MouseMoveEventHandler(UIElement sender, MouseMoveEventArgs args);

        /// <summary>
        /// Handler mouse button events.
        /// </summary>
        public delegate void MouseButtonEventHandler(UIElement sender, MouseButtonEventArgs args);

        /// <summary>
        /// Handler wheel events.
        /// </summary>
        public delegate void MouseWheelEventHandler(UIElement sender, MouseWheelEventArgs args);

        /// <summary>
        /// Handler keyboard events.
        /// </summary>
        public delegate void KeyboardKeyEventHandler(UIElement sender, KeyboardKeyEventArgs args);

        /// <summary>
        /// Handles text input events.
        /// </summary>
        public delegate void TextInputEventHandler(UIElement sender, TextInputEventArgs args);

        /// <summary>
        /// The mouse cursor entered this UI element.
        /// </summary>
        public event MouseMoveEventHandler? Entered;

        /// <summary>
        /// The mouse cursor left this UI element.
        /// </summary>
        public event MouseMoveEventHandler? Left;

        /// <summary>
        /// Any mousebutton was pressed while above this element.
        /// </summary>
        public event MouseButtonEventHandler? MouseButtonPressed;

        /// <summary>
        /// Any mousebutton was released while above this element.
        /// </summary>
        public event MouseButtonEventHandler? MouseButtonReleased;

        /// <summary>
        /// Occus when this element was clicked with any mouse button.
        /// </summary>
        public event MouseButtonEventHandler? Clicked;

        /// <summary>
        /// Occurs when this a scroll happened on this element.
        /// </summary>
        public event MouseWheelEventHandler? Scrolled;

        /// <summary>
        /// Occurs when this element gains keyboard focus.
        /// </summary>
        public event UIEventHandler? GainedFocus;

        /// <summary>
        /// Occurs when this element lost keyboard focus.
        /// </summary>
        public event UIEventHandler? LostFocus;

        /// <summary>
        /// Occurs when this elemet receives a text input.
        /// </summary>
        public event TextInputEventHandler? TextInput;

        /// <summary>
        /// Occurs when this element receives a key press.<br/>
        /// Use <see cref="TextInput"/> if you want to use the<br/>
        /// event argument as string input.
        /// </summary>
        public event KeyboardKeyEventHandler? KeyPressed;

        /// <summary>
        /// Occurs when this element receives a key release.
        /// </summary>
        public event KeyboardKeyEventHandler? KeyReleased;

        /// <summary>
        /// Wether or not this element has currently keyboard focus.
        /// </summary>
        public bool Focused { get; internal set; }

        /// <summary>
        /// Wether or not the mouse cursor is currently over this UI element.
        /// </summary>
        public bool MouseOver { get; internal set; }

        /// <summary>
        /// Wether or not any mouse button is down on this element.
        /// </summary>
        public bool MouseDown { get; internal set; }

        /// <summary>
        /// Saves if a click started inside this element.<br/>
        /// It is also a valid click if you drag out and reenter the element.
        /// </summary>
        public bool ClickInitiated { get; internal set; }

        internal void InvokeEntered(MouseMoveEventArgs args) => Entered?.Invoke(this, args);

        internal void InvokeLeft(MouseMoveEventArgs args) => Left?.Invoke(this, args);

        internal void InvokeMouseButtonPressed(MouseButtonEventArgs args) => MouseButtonPressed?.Invoke(this, args);

        internal void InvokeMouseButtonReleased(MouseButtonEventArgs args) => MouseButtonReleased?.Invoke(this, args);

        internal void InvokeClicked(MouseButtonEventArgs args) => Clicked?.Invoke(this, args);

        internal void InvokeScrolled(MouseWheelEventArgs args) => Scrolled?.Invoke(this, args);

        internal void InvokeGainedFocus() => GainedFocus?.Invoke(this);

        internal void InvokeLostFocus() => LostFocus?.Invoke(this);

        internal void InvokeKeyPressed(KeyboardKeyEventArgs args) => KeyPressed?.Invoke(this, args);

        internal void InvokeKeyReleased(KeyboardKeyEventArgs args) => KeyReleased?.Invoke(this, args);

        internal void InvokeTextInput(TextInputEventArgs args) => TextInput?.Invoke(this, args);

        #endregion
    }
}
