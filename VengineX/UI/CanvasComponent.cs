using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.ECS;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.Graphics.Rendering.Renderers;

namespace VengineX.UI
{
    // TODO get rid of this and make canvas a component instead somehow (canvas is currently inheriting from UI.Element).
    public class CanvasComponent : Component
    {
        private readonly Canvas _canvas;

        public CanvasComponent(Canvas canvas) : base(typeof(CanvasComponent)) => _canvas = canvas;

        public void UpdateEvents() => _canvas.UpdateEvents();

        public void UpdateLayout() => _canvas.UpdateLayout();

        public Camera Camera => _canvas.Camera;

        public IEnumerable<QuadVertex> EnumerateQuads() => _canvas.EnumerateQuads();


        public static implicit operator Canvas(CanvasComponent c) => c._canvas;
    }
}
