using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.Graphics.Rendering.Meshes;

namespace VengineX.Graphics.Rendering.Renderers
{
    /// <summary>
    /// Class that handles rendering of mesh components.<br/>
    /// Sorted by materials and then rendered.
    /// </summary>
    public class Renderer3D
    {
        private readonly Dictionary<Material, List<MeshComponent>> _meshes;
        private Camera? _camera;


        /// <summary>
        /// Creates a new renderer for 3D meshes.
        /// </summary>
        public Renderer3D()
        {
            _meshes = new Dictionary<Material, List<MeshComponent>>();
        }


        /// <summary>
        /// Begins the renderer.<br/>
        /// Submit all mesh components between <see cref="Begin(Camera)"/> and <see cref="End"/><br/>
        /// and render them by calling <see cref="Flush"/>.
        /// </summary>
        /// <param name="camera">Camera that provides view and projection matrix for the meshes.</param>
        public void Begin(Camera camera)
        {
            foreach (List<MeshComponent> meshList in _meshes.Values)
            {
                meshList.Clear();
            }

            _camera = camera;
        }


        /// <summary>
        /// Submits the given <see cref="MeshComponent"/> to the renderer.
        /// </summary>
        public void Submit(MeshComponent mesh)
        {
            if (_meshes.ContainsKey(mesh.Material))
            {
                _meshes[mesh.Material].Add(mesh);
            }
            else
            {
                _meshes.Add(mesh.Material, new List<MeshComponent>());
                _meshes[mesh.Material].Add(mesh);
            }
        }


        /// <summary>
        /// Ends the submitting.
        /// </summary>
        public void End() { }


        /// <summary>
        /// Renders all the <see cref="MeshComponent"/>s in the renderer.
        /// </summary>
        public void Flush()
        {
            if (_camera == null) { return; }

            foreach (KeyValuePair<Material, List<MeshComponent>> kvp in _meshes)
            {
                kvp.Key.Bind();
                kvp.Key.Shader.GetUniform("V").SetMat4(ref _camera.ViewMatrix);
                kvp.Key.Shader.GetUniform("P").SetMat4(ref _camera.ProjectionMatrix);

                foreach (MeshComponent mesh in kvp.Value)
                {
                    mesh.Material.Shader.GetUniform("M").SetMat4(ref mesh.Transform.ModelMatrix);
                    mesh.Render();
                }
            }
        }
    }
}
