using OpenTK.Mathematics;
using VengineX.Graphics.Rendering.Renderers;
using VengineX.UI.Layouts;

namespace VengineX.UI.Elements.Basic
{
    /// <summary>
    /// Abstract base class for all ui elements.<br/>
    /// Might also be used as a panel that controls the layout of its children.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Handles all events of this element.
        /// </summary>
        public EventEmitter Events { get; }

        /// <summary>
        /// The parent canvas of this ui element.
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// TParent of this element.
        /// </summary>
        public Element Parent { get; set; } = null;

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
        public float X
        {
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
        public Vector2 AbsolutePosition => Parent == null ? Position : Parent.AbsolutePosition + Position;

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
                foreach (Element child in Children)
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
        private List<Element> Children { get; }

        /// <summary>
        /// Calculates the preferred size of this element.<br/>
        /// Preferred size is the size of this element needed to fit all its layouted children.
        /// </summary>
        public Vector2 PreferredSize => Layout == null ? Size : Layout.PreferredSize(this);

        /// <summary>
        /// If this is set to true, this element will not receive ui events.
        /// </summary>
        public bool IgnoreInputEvents { get; set; } = false;


        /// <summary>
        /// Creates a new ui element with the given element as parent.
        /// </summary>
        public Element(Element? parent)
        {
            Events = new EventEmitter(this);
            Children = new List<Element>();

            if (parent != null)
            {
                parent.AddChild(this);
                Canvas = parent.Canvas;
            }
            else
            {
                if (GetType() == typeof(Canvas))
                {
                    Canvas = (Canvas)this;
                }
                else
                {
                    throw new ArgumentException("Parent can not be null, except for Canvas!");
                }
            }
        }


        /// <summary>
        /// Returns enumerator of <see cref="Children"/>.
        /// </summary>
        /// <param name="recursive">
        /// If set to true, it will recursively enumerate the children. And return all UIElements that are<br/>
        /// nested in this element.
        /// </param>
        public IEnumerable<Element> EnumerateChildren(bool recursive = false)
        {
            if (recursive)
            {
                return AllChildren();
            }
            else
            {
                return Children;
            }
        }


        /// <summary>
        /// Iterates over the tree of children.
        /// </summary>
        protected virtual IEnumerable<Element> AllChildren()
        {
            foreach (Element child in Children)
            {
                yield return child;

                foreach (Element childsChild in child.AllChildren())
                {
                    yield return childsChild;
                }
            }
        }


        /// <summary>
        /// Adds child element at given index.
        /// </summary>
        public virtual void AddChild(int index, Element element)
        {
            Children.Insert(index, element);
            element.Parent = this;
        }


        /// <summary>
        /// Adds child at the end.
        /// </summary>
        public void AddChild(Element element)
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
        public void RemoveChild(Element element) => Children.Remove(element);


        /// <summary>
        /// Returns the index of given child.
        /// </summary>
        public void IndexOf(Element element) => Children.IndexOf(element);


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
            foreach (Element child in EnumerateChildren())
            {
                child.UpdateLayout();
            }

            // Update own layout (layouts all direct children)
            Layout?.UpdateLayout(this);
        }


        /// <summary>
        /// Generates and returns the <see cref="QuadVertex"/>s for this element and all its children.
        /// </summary>
        public virtual IEnumerable<QuadVertex> EnumerateQuads()
        {
            foreach (Element child in EnumerateChildren())
            {
                foreach (QuadVertex quadVertex in child.EnumerateQuads())
                {
                    yield return quadVertex;
                }
            }
        }
    }
}
