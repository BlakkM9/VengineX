using OpenTK.Mathematics;
using VengineX.Debugging.Logging;
using VengineX.Graphics.Shaders;
using VengineX.Graphics.Textures;

namespace VengineX.Graphics
{
    /// <summary>
    /// A material groups a shader and materials together.
    /// </summary>
    public class Material : IBindable, IEquatable<Material?>
    {
        /// <summary>
        /// The shader used by this material.
        /// </summary>
        public Shader Shader { get; }

        /// <summary>
        /// The textures of this material with the uniform name as key.
        /// </summary>
        private readonly Dictionary<string, Texture> _textures;

        /// <summary>
        /// Array that manages the texture slot for each texture.<br/>
        /// The index represents the texture slot to use for the texture on that index.
        /// </summary>
        private readonly Texture?[] _textureSlots;


        /// <summary>
        /// Creates a new empty material
        /// </summary>
        /// <param name="shader"></param>
        public Material(Shader shader)
        {
            Shader = shader;
            _textures = new Dictionary<string, Texture>();
            _textureSlots = new Texture[32];
        }


        /// <summary>
        /// Adds a texture with given uniform name to this material.
        /// </summary>
        public void AddTexture(string uniformName, Texture texture)
        {
            // Add to dictionary for user access
            _textures.Add(uniformName, texture);

            // Find next available texture slot
            int slot = -1;
            for (int i = 0; i < _textureSlots.Length; i++)
            {
                if (_textureSlots[i] == null)
                {
                    _textureSlots[i] = texture;
                    slot = i;
                    break;
                }
            }

            if (slot == -1)
            {
                Logger.Log(Severity.Error, "No free texture slot found!");
                return;
            }

            // Set texture uniform to assigned slot
            Shader.GetUniform(uniformName).Set1(slot);
        }


        /// <summary>
        /// Removes the texture for given uniform name.
        /// </summary>
        public void RemoveTexture(string uniformName)
        {
            // Remove from dict
            _textures.Remove(uniformName);

            // Remove from slots
            for (int i = 0; i < _textureSlots.Length; i++)
            {
                _textureSlots[i] = null;
            }
        }


        /// <summary>
        /// Binds the shader and all the textures of this material to the current<br/>
        /// opengl renderer state.
        /// </summary>
        public void Bind()
        {
            // Bind the used shader
            Shader.Bind();

            // Bind each texture to previously assinged slot
            for (uint i = 0; i < _textureSlots.Length; i++)
            {
                _textureSlots[i]?.Bind(i);
            }
        }


        /// <summary>
        /// Unbinds this material from the current opengl renderer state.<br/>
        /// Unbinds shader and textures.
        /// </summary>
        public void Unbind()
        {
            // Unbind shader
            Shader.Unbind();

            // Unbind each texture
            for (uint i = 0; i < _textureSlots.Length; i++)
            {
                _textureSlots[i]?.Unbind();
            }
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as Material);
        }


        public bool Equals(Material? other)
        {
            return other != null &&
                   EqualityComparer<Shader>.Default.Equals(Shader, other.Shader) &&
                   EqualityComparer<Dictionary<string, Texture>>.Default.Equals(_textures, other._textures) &&
                   EqualityComparer<Texture?[]>.Default.Equals(_textureSlots, other._textureSlots);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Shader, _textures, _textureSlots);
        }
    }
}
