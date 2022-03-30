using VengineX.Resources;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Fonts
{
    /// <summary>
    /// Parameters for loading a bitmap font via FreeType.
    /// </summary>
    public struct BitmapFontLoadingParameters : ILoadingParameters
    {
        /// <summary>
        /// The first token of automaticall generated resource strings.
        /// </summary>
        private const string RESOURCE_TYPE = "font";

        /// <summary>
        /// The path to the font.
        /// </summary>
        public string FontPath { get; set; }

        /// <summary>
        /// The <see cref="CharacterRange"/>s that should be loaded of this font.
        /// </summary>
        public CharacterRange[] Ranges { get; set; }

        /// <summary>
        /// The size to load this font in.<br/>
        /// One character will be loaded as a size * size bitmap.
        /// </summary>
        public int Size { get; set; }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ProvideResourcePath()
        {
            // Split path
            string[] tokens = FontPath.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Strip extension
            string fileName = tokens[^1];
            fileName = fileName[..fileName.IndexOf('.')];

            // Join RESOURCE_TYPE, path except res + rootFolder and fileName
            List<string> resourceTokens = new List<string>();
            resourceTokens.Add(RESOURCE_TYPE);
            resourceTokens.AddRange(tokens[2..^1]);
            resourceTokens.Add(fileName);
            string resourcePath = string.Join(".", resourceTokens);

            return resourcePath.ToLower();
        }
    }
}
