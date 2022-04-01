using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VengineX.Core;
using VengineX.ECS;
using VengineX.Graphics;
using VengineX.Graphics.Meshes;
using VengineX.Graphics.Shaders;
using VengineX.Graphics.Textures;
using VengineX.Graphics.Vertices;

namespace VengineX.Graphics.UnitModels
{
    /// <summary>
    /// A cube with size 1, origin at center.<br/>
    /// Create and attach to registry for testing.<br/>
    /// Do not use this for creating larger structures like chunks from this.<br/>
    /// Do not forget to dispose after usage.
    /// </summary>
    public class UnitCube : Entity, IDisposable
    {
        /// <summary>
        /// Vertices for the unit cube (<see cref="PBRVertex"/>)
        /// </summary>
        private static readonly PBRVertex[] _vertices =
        {
            // BACK
            new PBRVertex()
            {
                position = new Vector3(-0.5f, 0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, 0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, -0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, -0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },

            // FRONT
            new PBRVertex()
            {
                position = new Vector3(-0.5f, -0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, -0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, 0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, 0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },

            // TOP
            new PBRVertex()
            {
                position = new Vector3(-0.5f,  0.5f,  -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f,  0.5f,  0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f,  0.5f,  -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f,  0.5f,  0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },

            // BOTTOM
            new PBRVertex()
            {
                position = new Vector3(0.5f, -0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, -0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, -0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, -0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },

            // LEFT
            new PBRVertex()
            {
                position = new Vector3(-0.5f, -0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, 0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, -0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(-0.5f, 0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },

            // RIGHT
            new PBRVertex()
            {
                position = new Vector3(0.5f, -0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, 0.5f, -0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(1.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, -0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 0.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
            new PBRVertex()
            {
                position = new Vector3(0.5f, 0.5f, 0.5f),
                normal = new Vector3(0.0f, 0.0f, 1.0f),
                tangent = new Vector3(-1.0f, 0.0f, 0.0f),
                uv = new Vector3(0.0f, 1.0f, 0.0f),
                color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            },
        };

        /// <summary>
        /// Indices for the unit cube.
        /// </summary>
        private static readonly uint[] _indices =
        {
            // back
            0, 1, 2,
            2, 1, 3,

            // front
            4, 5, 6,
            6, 5, 7,

            // top
            8, 9, 10,
            10, 9, 11,

            // bottom
            12, 13, 14,
            14, 13, 15,

            // left
            16, 17, 18,
            18, 17, 19,

            // right
            20, 21, 22,
            22, 21, 23,
        };

        /// <summary>
        /// Transform of the cube.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// Mesh component of the cube.
        /// </summary>
        public MeshComponent MeshComponent { get; }


        /// <summary>
        /// Creates a new unit cube entity with the given material.
        /// </summary>
        public UnitCube(Material material)
        {
            // Create transform
            Transform = new Transform();
            AddComponent(Transform);

            // Create mesh, material and mesh component
            Mesh<PBRVertex> mesh = new Mesh<PBRVertex>(
                BufferUsageHint.StaticDraw,
                BufferUsageHint.StaticDraw,
                _vertices,
                _indices);

            MeshComponent = new MeshComponent(mesh, material);
            AddComponent(MeshComponent);
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
                    MeshComponent.Mesh.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }

        // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TestCube()
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
