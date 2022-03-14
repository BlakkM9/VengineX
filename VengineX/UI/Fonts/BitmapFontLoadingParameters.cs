using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Resources;

namespace VengineX.UI.Fonts
{
    /// <summary>
    /// Parameters for loading a bitmap font via FreeType.
    /// </summary>
    public struct BitmapFontLoadingParameters : ILoadingParameters
    {
        public string FontPath { get; set; }
        public byte FromCharCode { get; set; }
        public byte ToCharCode { get; set; }
        public int Size { get; set; }
    }
}
