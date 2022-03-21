using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Cameras;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.Graphics.Rendering
{
    public class Material : IBindable, IEquatable<Material>
    {
        public Shader Shader { get; }
        public Texture2D[] Textures { get; }

        public int ModelMatrixLocation { get; }
        public int ViewMatrixLocation { get; }
        public int ProjectionMatrixLocation { get; }


        public Material(Shader shader, params Texture2D[] textures)
        {
            Shader = shader;
            ModelMatrixLocation = Shader.GetUniformLocation("M");
            ViewMatrixLocation = Shader.GetUniformLocation("V");
            ProjectionMatrixLocation = Shader.GetUniformLocation("P");

            Textures = new Texture2D[32];

            for (int i = 0; i < textures.Length; i++)
            {
                Textures[i] = textures[i];
            }
        }


        public void UpdateCameraMatrixUniforms(ref Matrix4 viewMatrix, ref Matrix4 projectionMatrix)
        {
            Shader.SetUniformMat4(ViewMatrixLocation, ref viewMatrix);
            Shader.SetUniformMat4(ProjectionMatrixLocation, ref projectionMatrix);
        }


        public virtual void UpdateModelMatrix(ref Matrix4 modelMatrix)
        {
            Shader.SetUniformMat4(ModelMatrixLocation, ref modelMatrix);
        }


        public void Bind()
        {
            Shader.Bind();

            for (uint i = 0; i < Textures.Length; i++)
            {
                Textures[i]?.Bind(i);
            }
        }


        public void Unbind()
        {

        }


        public bool Equals(Material? other)
        {
            if (other == this) { return true; }
            if (other == null) { return false; }

            for (int i = 0; i < Textures.Length; i++)
            {
                if (other.Textures[i].Handle != Textures[i].Handle) { return false; }
            }

            return Shader.Handle == other.Shader.Handle;
        }
    }
}
