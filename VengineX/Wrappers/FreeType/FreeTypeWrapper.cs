using System.Runtime.InteropServices;
using System.Text;
using VengineX.Debugging.Logging;

namespace VengineX.Wrappers.FreeType
{
    public static class FreeTypeWrapper
    {

        private const string dllFilePath = "lib/freetypelib.dll";


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static int Init_FreeType();


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static int Done_FreeType();


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static FreeTypeGlyph* Load_Glyphs(ref byte str, ref CharacterRange ranges, int rangesCount, int fontSize);


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static int Free_Glyphs(FreeTypeGlyph* glyphsToFree, int length);


        /// <summary>
        /// Initializes freetype. Call before loading glyphs.
        /// </summary>
        public static void InitFreeType()
        {
            if (!Initialized)
            {
                int errCode = Init_FreeType();
                if (errCode == 0)
                {
                    Initialized = true;
                    Logger.Log(Severity.Info, Tag.Initialization, "FreeType initialized.");
                }
                else
                {
                    Logger.Log(Severity.Error, Tag.Initialization, $"Failed to initialize FreeType with error code {errCode}");
                }
            }
            else
            {
                Logger.Log(Severity.Warning, "FreeType already initialized!");
            }
        }

        /// <summary>
        /// Is FreeType Initialized?
        /// </summary>
        public static bool Initialized { get; private set; } = false;


        /// <summary>
        /// Loads glyphs from a true type font using free type font.
        /// </summary>
        /// <param name="filePath">Path to font file.</param>
        /// <param name="characterRanges">Ranges of characters to load.</param>
        /// <param name="size">Font size in px.</param>
        /// <returns>Unamanged array holding all loaded glyphs. Dont release it, use <see cref="FreeGlyphs(FreeTypeGlyph*, int)"/> instead!</returns>
        public static unsafe FreeTypeGlyph* LoadGlyphs(string filePath, CharacterRange[] characterRanges, int size)
        {
            if (!Initialized)
            {
                InitFreeType();
            }

            // Convert file path to c string
            byte[] bytes = Encoding.ASCII.GetBytes(filePath);

            // Load glyphs with free type
            FreeTypeGlyph* glyphs = Load_Glyphs(ref bytes[0], ref characterRanges[0], characterRanges.Length, size);

            return glyphs;
        }


        /// <summary>
        /// Deletes the unmanaged glyph array.<br/>
        /// Call this if you don't need the loaded glyphs anymore.<br/>
        /// Not calling this will lead to memory leaks.
        /// </summary>
        /// <param name="glyphsToFree">The unmanaged glyphs array to free.</param>
        /// <param name="length">Lenght of the unmanaged array.</param>
        public static unsafe void FreeGlyphs(FreeTypeGlyph* glyphsToFree, int length)
        {
            _ = Free_Glyphs(glyphsToFree, length);
        }


        /// <summary>
        /// Uninitializes freetype.<br/>
        /// Call when you are done using freetype.
        /// </summary>
        public static void DoneFreeType()
        {
            if (Initialized)
            {
                _ = Done_FreeType();
                Initialized = false;
            }
        }
    }
}
