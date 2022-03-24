using OpenTK.Mathematics;

namespace VengineX.Wrappers.FreeType
{
    public struct Character
    {
        public bool HasTexture { get; set; }
        public Vector2i Size { get; set; }
        public Vector2i Bearing { get; set; }
        public uint Advance { get; set; }
        public Vector2[] UVs { get; set; }
    }
}
