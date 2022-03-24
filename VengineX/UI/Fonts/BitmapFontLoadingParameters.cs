using VengineX.Resources;
using VengineX.Wrappers.FreeType;

namespace VengineX.UI.Fonts
{
    /// <summary>
    /// Parameters for loading a bitmap font via FreeType.
    /// </summary>
    public struct BitmapFontLoadingParameters : ILoadingParameters
    {
        public string FontPath { get; set; }
        public CharacterRange[] Ranges { get; set; }
        public int Size { get; set; }
    }
}
