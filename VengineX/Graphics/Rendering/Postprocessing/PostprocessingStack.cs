using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Graphics.Rendering.Buffers;
using VengineX.Graphics.Rendering.Shaders;
using VengineX.Graphics.Rendering.UnitModels;
using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Postprocessing
{
    
    public class PostprocessingStack
    {
        /// <summary>
        /// Dictionary mapping the string identifier of the post processing effect<br/>
        /// to the index of the effect in <see cref="_effects"/>.
        /// </summary>
        private readonly Dictionary<string, int> _effectMap;

        /// <summary>
        /// List holding all the effects that should be applied in the desired order of application.
        /// </summary>
        private readonly List<PostprocessingEffect> _effects;

        /// <summary>
        /// Shader that is used to render the framebuffers texture to the screen.
        /// </summary>
        private readonly Shader _postprocessingShader;

        /// <summary>
        /// Framebuffer that is used for offscreen rendering the effects and provides the initial screen texture.
        /// </summary>
        private readonly Framebuffer2D _framebuffer;


        /// <summary>
        /// Returns the <see cref="PostprocessingEffect"/> added with the given name.<br/>
        /// </summary>
        public PostprocessingEffect this[string name]
        {
            get => _effects[_effectMap[name]];
        }

        /// <summary>
        /// Creates a new post-processing stack.
        /// </summary>
        /// <param name="framebuffer">
        /// The framebuffer this stack should use for off-screen rendering<br/>
        /// and receiving the screen texture
        /// </param>
        public PostprocessingStack(Framebuffer2D framebuffer)
        {
            _effectMap = new Dictionary<string, int>();
            _effects = new List<PostprocessingEffect>();
            _postprocessingShader = ResourceManager.GetResource<Shader>("shader.postprocessing.postprocessing");
            _framebuffer = framebuffer;
        }


        /// <summary>
        /// Applys all enabled postprocessing effects in this stack and renders<br/>
        /// The output to the screen.
        /// </summary>
        public void ApplyAll()
        {
            GL.Disable(EnableCap.DepthTest);

            foreach (PostprocessingEffect effect in _effects)
            {
                if (effect.Enabled) { effect.Apply(_framebuffer); }
            }

            RenderToScreen();

            GL.Enable(EnableCap.DepthTest);
        }


        /// <summary>
        /// Adds a post processing effect to the end of the stack.
        /// </summary>
        public void AddEffect(string name, PostprocessingEffect effect)
        {
            _effects.Add(effect);
            _effectMap.Add(name, _effects.Count - 1);
        }


        /// <summary>
        /// Renders the current framebuffers texture to the screen.
        /// </summary>
        private void RenderToScreen()
        {
            // Render to screen
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _postprocessingShader.Bind();
            _framebuffer.OutputTexture.Bind();
            UnitQuad.Render();
        }
    }
}
