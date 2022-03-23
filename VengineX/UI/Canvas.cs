﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering;
using VengineX.Graphics.Rendering.Batching;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.UnitModels;
using VengineX.Input;
using VengineX.Resources;
using VengineX.UI.Elements.Basic;
using VengineX.UI.Fonts;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI
{
    /// <summary>
    /// This class is the top element of any ui.<br/>
    /// All ui elements and events are handled from this class downwards.
    /// </summary>
    public class Canvas : Element, IRenderable, IDisposable
    {
        public ref Matrix4 ModelMatrix => ref _modelMatrix;
        private Matrix4 _modelMatrix = Matrix4.Identity;

        /// <summary>
        /// The Sprite batch renderer that is used to draw the ui.
        /// </summary>
        public UIBatchRenderer BatchRenderer { get; private set; }

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
        /// The <see cref="UI.EventSystem"/> that handles (and indirectly invokes) all the<br/>
        /// input events for ui elements within this canvas.
        /// </summary>
        public EventSystem EventSystem { get; }


        /// <summary>
        /// Creates a new canvas to render UI elements on.
        /// </summary>
        public Canvas(float width, float height, InputManager input) : base(null)
        {
            BatchRenderer = new UIBatchRenderer(1000);
            EventSystem = new EventSystem(input, this);
            Size = new Vector2(width, height);

            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -1, 1);
            BatchRenderer.SetMatrices(ref ProjectionMatrix, ref ViewMatrix);
        }


        /// <summary>
        /// Call every frame to update mouse move events<br/>
        /// for the elements in this canvas.
        /// </summary>
        public void UpdateEvents()
        {
            EventSystem.UpdateMouseMove();
        }



        public void Render()
        {
            BatchRenderer.Begin();

            foreach (Element child in AllChildren())
            {
                if (child.Visible)
                {
                    foreach (UIBatchQuad element in child.EnumerateQuads())
                    {
                        BatchRenderer.Add(element);
                    }
                }
            }

            BatchRenderer.End();
            BatchRenderer.Flush();
        }


        /// <summary>
        /// Resizes the canvas.
        /// </summary>
        public void Resize(float newWidth, float newHeight)
        {
            Size = new Vector2(newWidth, newHeight);
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, 0, Height, -1, 1);
            BatchRenderer.SetMatrices(ref ProjectionMatrix, ref ViewMatrix);
        }


        public override IEnumerable<UIBatchQuad> EnumerateQuads()
        {
            yield break;
        }


        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    BatchRenderer.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }

        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
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
