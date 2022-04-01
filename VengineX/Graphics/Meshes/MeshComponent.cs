using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Debugging.Logging;
using VengineX.ECS;

namespace VengineX.Graphics.Meshes
{
    /// <summary>
    /// The component for meshes that are rendered in the screen.
    /// </summary>
    public class MeshComponent : Component
    {
        /// <summary>
        /// The internal mesh to use for the component.
        /// </summary>
        public MeshBase Mesh { get; }

        /// <summary>
        /// The material to use for rendering the mesh.
        /// </summary>
        public Material Material { get; }

        /// <summary>
        /// Gets or sets if this mesh component should be rendered or not.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// The transform that determines the world position of the mesh.<br/>
        /// This is the transform of the parent entity.
        /// </summary>
        public Transform? Transform { get; private set; }

        /// <summary>
        /// Creates a new mesh component. 
        /// </summary>
        /// <param name="mesh">The internal mesh to use for the component.</param>
        /// <param name="material">The material to use for rendering the mesh.</param>
        public MeshComponent(MeshBase mesh, Material material) : base(typeof(MeshComponent))
        {
            Mesh = mesh;
            Material = material;
            Visible = true;
        }


        /// <summary>
        /// Renders the mesh.
        /// </summary>
        public void Render() => Mesh.Render();


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Attached()
        {
            Transform = Entity?.GetComponent<Transform>();

            if (Transform == null)
            {
                Logger.Log(Severity.Fatal, "A mesh can only be attached to components that have a Transform component attached already!");
            }
        }


        /// <summary>
        /// Implicit cast to <see cref="MeshBase"/>.
        /// </summary>
        public static implicit operator MeshBase(MeshComponent m) => m.Mesh;
    }
}
