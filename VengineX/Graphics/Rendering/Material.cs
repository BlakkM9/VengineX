using OpenTK.Graphics.OpenGL4;
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
    public class Material : IBindable
    {
        public Shader Shader { get; }
        public Texture2D[] Textures { get; }

        public int ModelMatrixLocation { get; }
        public int ViewMatrixLocation { get; }
        public int ProjectionMatrixLocation { get; }


        public Material(Shader shader, Texture2D[] textures)
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


        public void UpdateCameraMatrixUniforms(Camera camera)
        {
            Shader.SetUniformMat4(ViewMatrixLocation, ref camera.ViewMatrix);
            Shader.SetUniformMat4(ProjectionMatrixLocation, ref camera.ProjectionMatrix);
        }


        public virtual void UpdateRenderableUniforms(IRenderable renderable)
        {
            Shader.SetUniformMat4(ModelMatrixLocation, ref renderable.ModelMatrix);
        }


        public void Bind()
        {
            Shader.Bind();

            for (int i = 0; i < Textures.Length; i++)
            {
                Textures[i]?.Bind(i);
            }
        }


        public void Unbind()
        {

        }
    }
}
