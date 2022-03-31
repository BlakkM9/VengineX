using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Buffers;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.Textures;
using VengineX.Graphics.Rendering.UnitModels;

namespace VengineX.Graphics.Rendering.Postprocessing
{
    /// <summary>
    /// This class represents a single (one pass) postprocessing effect that is applied by adding and applying<br/>
    /// from a <see cref="PostprocessingStack"/>. You can derive from that class if you want to create more complex<br/>
    /// post processing effects (like multiple passes/multiple shaders etc).
    /// </summary>
    public class PostprocessingEffect
    {
        /// <summary>
        /// The shader that is used to create the postprocessing effect.
        /// </summary>
        public Shader Shader { get; }

        /// <summary>
        /// Wether this post-processing effect is currently enabled or not.
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// Creates a new postprocessing effect with given shader.
        /// </summary>
        /// <param name="postprocessingShader">Shader that is used to create the effect.</param>
        public PostprocessingEffect(Shader postprocessingShader)
        {
            Shader = postprocessingShader;
            Enabled = true;
        }


        /// <summary>
        /// Applies this postprocessing effect to the given framebuffer.<br/>
        /// Input and output is the framebuffers output texture.
        /// </summary>
        public virtual void Apply(Framebuffer2D framebuffer)
        {
            Shader.Bind();
            framebuffer.OutputTexture.Bind();

            framebuffer.Bind();
            UnitQuad.Render();
            framebuffer.Unbind();
        }
    }
}
