using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Graphics.Cameras;
using VengineX.Graphics.Meshes;
using VengineX.Utils.Extensions;

namespace VengineX.Graphics.Renderers
{
    /// <summary>
    /// Class that handles rendering of mesh components.<br/>
    /// Sorted by materials and then rendered.
    /// </summary>
    public class Renderer3D
    {
        public int Triangles { get; protected set; }

        public int DrawCalls { get; protected set; }


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
            Triangles = 0;

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
            Triangles += mesh.Mesh.IndexCount / 3;

            if (!_meshes.ContainsKey(mesh.Material))
            {
                _meshes.Add(mesh.Material, new List<MeshComponent>());
            }

            _meshes[mesh.Material].Add(mesh);
        }


        /// <summary>
        /// Ends the submitting.
        /// </summary>
        public void End() {

            //// TODO TEMP
            //foreach (Material mat in _meshes.Keys)
            //{
                //Vector3 camPos = (Vector3)_camera.Transform.Position;
                //mat.Shader.GetUniform("uCameraPositionWS").Set3(ref camPos);
            //}
        }


        /// <summary>
        /// Renders all the <see cref="MeshComponent"/>s in the renderer.
        /// </summary>
        public void Flush()
        {
            if (_camera == null) { return; }

            DrawCalls = 0;

            foreach (KeyValuePair<Material, List<MeshComponent>> kvp in _meshes)
            {
                kvp.Key.Bind();
                Matrix4 view = _camera.ViewMatrix.ToMatrix4();
                Matrix4 proj = _camera.ProjectionMatrix.ToMatrix4();
                kvp.Key.Shader.GetUniform("V").SetMat4(ref view);
                kvp.Key.Shader.GetUniform("P").SetMat4(ref proj);

                foreach (MeshComponent mesh in kvp.Value)
                {
                    //Matrix4 model = mesh.Transform.ModelMatrix.ToMatrix4();
                    Matrix4 viewModel = (mesh.Transform.ModelMatrix * _camera.ViewMatrix).ToMatrix4();
                    //Matrix4 pvm = (mesh.Transform.ModelMatrix * _camera.ViewMatrix * _camera.ProjectionMatrix).ToMatrix4();

                    mesh.Material.Shader.GetUniform("VM").SetMat4(ref viewModel);
                    //mesh.Material.Shader.GetUniform("PVM").SetMat4(ref pvm);
                    //mesh.Material.Shader.GetUniform("M").SetMat4(ref model);
                    mesh.Render();
                    DrawCalls++;
                }
            }
        }
    }
}
