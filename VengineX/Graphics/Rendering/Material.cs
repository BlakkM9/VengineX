using OpenTK.Mathematics;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;

namespace VengineX.Graphics.Rendering
{
    public class Material : IBindable, IEquatable<Material>
    {
        public Shader Shader { get; }
        public Texture2D[] Textures { get; }

        public Uniform ModelMatrixUniform { get; }
        public Uniform ViewMatrixUniform { get; }
        public Uniform ProjectionMatrixUniform { get; }


        public Material(Shader shader, params Texture2D[] textures)
        {
            Shader = shader;
            ModelMatrixUniform = Shader.GetUniform("M");
            ViewMatrixUniform = Shader.GetUniform("V");
            ProjectionMatrixUniform = Shader.GetUniform("P");

            Textures = new Texture2D[32];

            for (int i = 0; i < textures.Length; i++)
            {
                Textures[i] = textures[i];
            }
        }


        public void UpdateCameraMatrixUniforms(ref Matrix4 viewMatrix, ref Matrix4 projectionMatrix)
        {
            //Shader.SetUniformMat4(ViewMatrixUniform, ref viewMatrix);
            //Shader.SetUniformMat4(ProjectionMatrixUniform, ref projectionMatrix);
            ProjectionMatrixUniform.SetMat4(ref projectionMatrix);
            ViewMatrixUniform.SetMat4(ref viewMatrix);
        }


        public virtual void UpdateModelMatrix(ref Matrix4 modelMatrix)
        {
            ModelMatrixUniform.SetMat4(ref modelMatrix);
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
