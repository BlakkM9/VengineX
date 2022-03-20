using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.UnitModels;
using VengineX.Input;
using VengineX.UI.Elements;

namespace VengineX.UI
{
    /// <summary>
    /// This class is the top element of any ui.<br/>
    /// All ui elements and events are handled from this class downwards.
    /// </summary>
    public class Canvas : UIElement, IRenderable
    {
        /// <summary>
        /// The projection matrix of this canvas
        /// </summary>
        public ref Matrix4 ProjectionMatrix { get => ref _projectionMatrix; }
        private Matrix4 _projectionMatrix;

        /// <summary>
        /// The view matrix of this canvas. I don't think this is actually needed.
        /// </summary>
        public ref Matrix4 ViewMatrix { get => ref _viewMatrix; }
        private Matrix4 _viewMatrix = Matrix4.Identity;

        /// <summary>
        /// The <see cref="UIEventSystem"/> that handles (and indirectly invokes) all the<br/>
        /// input events for ui elements within this canvas.
        /// </summary>
        public UIEventSystem EventSystem { get; }

        /// <summary>
        /// The quad for rendering all the UI elements (that can be rendered onto a quad).
        /// </summary>
        public Quad Quad { get; }


        /// <summary>
        /// Creates a new canvas to render UI elements on.
        /// </summary>
        public Canvas(float width, float height, InputManager input) : base(null)
        {
            EventSystem = new UIEventSystem(input, this);
            Size = new Vector2(width, height);
            Quad = new Quad();
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, -Height, 0, -1.0f, 1.0f);
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
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -1.0f, 1.0f);
        }
    }
}
