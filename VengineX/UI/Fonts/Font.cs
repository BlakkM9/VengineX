using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Resources;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Fonts
{
    /// <summary>
    /// Base class for all fonts.
    /// </summary>
    public abstract class Font : IDisposable, IResource, ILoadableResource
    {

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ResourcePath { get; set; } = string.Empty;

        /// <summary>
        /// Dictionary holding all the <see cref="Character"/> that are loaded.
        /// </summary>
        protected Dictionary<char, Character> Characters { get; }
        
        /// <summary>
        /// The size (height) of the font that was loaded in pixels.
        /// </summary>
        public float Size { get; protected set; }


        /// <summary>
        /// Creates a new empty font object, ready to be loaded (for <see cref="ResourceManager"/>).
        /// </summary>
        public Font()
        {
            Characters = new Dictionary<char, Character>();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void Load(ref ILoadingParameters loadingParameters);


        /// <summary>
        /// Calculates the width of the given text at given text size.
        /// </summary>
        public float CalculateWidth(string text, float size)
        {
            float width = 0;
            foreach (char c in text)
            {
                width += Characters[c].Advance >> 6;
            }
            return width * (size / Size);
        }

        public abstract void Dispose();
    }
}
