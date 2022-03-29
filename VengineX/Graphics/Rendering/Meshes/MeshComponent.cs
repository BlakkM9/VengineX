using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.ECS;

namespace VengineX.Graphics.Rendering.Meshes
{
    public class MeshComponent : Component
    {
        private BaseMesh _mesh { get; }
        public Material Material { get; }
        public Transform Transform { get; }

        public MeshComponent(BaseMesh mesh, Material material)
        {
            _mesh = mesh;
            Material = material;
            Transform = new Transform();
        }

        public void Render() => _mesh.Render();

        public static implicit operator BaseMesh(MeshComponent m) => m._mesh;
    }
}
