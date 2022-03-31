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
    public class Renderer3D
    {

        private Dictionary<Material, List<MeshComponent>> _meshes;
        private Camera _camera;

        public Renderer3D()
        {
            _meshes = new Dictionary<Material, List<MeshComponent>>();
        }


        public void Begin(Camera camera)
        {
            foreach (List<MeshComponent> meshList in _meshes.Values)
            {
                meshList.Clear();
            }

            _camera = camera;
        }



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


        public void End() { }


        public void Flush()
        {
            
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
