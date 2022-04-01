using OpenTK.Mathematics;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.Graphics.Rendering.Renderers;
using VengineX.Input;
using VengineX.UI.Elements.Basic;
using VengineX.UI.Layouts;

namespace VengineX.UI
{
    /// <summary>
    /// This class is the top element of any ui.<br/>
    /// All ui elements and events are handled from this class downwards.
    /// </summary>
    public class Canvas : Element
    {
        /// <summary>
        /// Camera that is used to render this canvas.
        /// </summary>
        public OrthographicCamera Camera { get; private set; }

        /// <summary>
        /// The <see cref="UI.EventSystem"/> that handles (and indirectly invokes) all the<br/>
        /// input events for ui elements within this canvas.
        /// </summary>
        public EventSystem EventSystem { get; }


        /// <summary>
        /// Creates a new canvas to render UI elements on.
        /// </summary>
        public Canvas(float width, float height, InputManager input) : base(null)
        {
            //BatchRenderer = new BatchRenderer2D(1000);
            EventSystem = new EventSystem(input, this);
            Size = new Vector2(width, height);

            Camera = new OrthographicCamera(Width, Height, -1, 1, true);
            Layout = new AlignLayout(HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
        }


        /// <summary>
        /// Call every frame to update mouse move events<br/>
        /// for the elements in this canvas.
        /// </summary>
        public void UpdateEvents()
        {
            EventSystem.UpdateMouseMove();
        }


        /// <summary>
        /// Resizes the canvas.
        /// </summary>
        public void Resize(float newWidth, float newHeight)
        {
            Size = new Vector2(newWidth, newHeight);
            Camera = new OrthographicCamera(newWidth, newHeight, -1, 1, true);
        }
    }
}
