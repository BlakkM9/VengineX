using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.UnitModels;
using VengineX.Input;
using VengineX.Resources;
using VengineX.UI.Elements;

namespace VengineX.UI.Canvases
{
    /// <summary>
    /// Canvas is the root of any UI.<br/>
    /// It is re-rendered onto famebuffers texture if needed.
    /// </summary>
    public class Canvas : UIElement, IDisposable
    {
        // TEST ONLY!
        public Shader UIShader { get; }
        public int colorLocation;
        public int pLoc;
        public int mLoc;
        public int vLoc;

        /// <summary>
        /// The input manager that controls all the UI elements within this canvas.
        /// </summary>
        public InputManager Input { get; private set; }

        // TODO batch all the ui elements into a single mesh if possible, maybe?
        /// <summary>
        /// The quad for rendering all the UI elements (that can be rendered onto a quad).
        /// </summary>
        public Quad Quad { get; }

        /// <summary>
        /// The projection matrix of this canvas
        /// </summary>
        public ref Matrix4 ProjectionMatrix { get => ref _projectionMatrix; }
        private Matrix4 _projectionMatrix;

        /// <summary>
        /// Width of the canvas.
        /// </summary>
        public override float Width { get; set; }

        /// <summary>
        /// Height of the canvas.
        /// </summary>
        public override float Height { get; set; }

        // TODO implement
        /// <summary>
        /// Inner padding of the canas.<br/>
        /// Padding means that the Children can only be moved<br/>
        /// within the inner space even if the canvas is larger than that.
        /// </summary>
        public float Padding { get; set; }


        public Canvas(float width, float height, InputManager input) : base(0, 0, width, height)
        {
            Input = input;
            Width = width;
            Height = height;
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, -Height, 0, -1.0f, 1.0f);
            ParentCanvas = this;
            ParentElement = this;

            Quad = new Quad();

            // TEST LOAD UI SHADER
            UIShader = ResourceManager.GetResource<Shader>("shader.ui");
            colorLocation = UIShader.GetUniformLocation("uTint");
            pLoc = UIShader.GetUniformLocation("P");
            mLoc = UIShader.GetUniformLocation("M");
            vLoc = UIShader.GetUniformLocation("V");

            UIShader.SetUniformMat4(pLoc, ref ProjectionMatrix);
            Matrix4 v = Matrix4.Identity;
            UIShader.SetUniformMat4(vLoc, ref v);
        }


        public override void Update()
        {
            // Ignore input when mouse is catched
            if (!Input.MouseCatched)
            {
                // Update children
                foreach (UIElement child in Children)
                {
                    child.Update();
                }
            }
        }


        public override void Render()
        {
            foreach (UIElement child in Children)
            {
                child.Render();
            }
        }


        public void Resize(float newWidth, float newHeight)
        {
            Width = newWidth;
            Height = newHeight;
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -1.0f, 1.0f);
        }

        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Quad.Dispose();
                    ResourceManager.UnloadResource(UIShader);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Canvas()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
