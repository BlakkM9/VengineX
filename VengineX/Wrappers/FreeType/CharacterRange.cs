using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Wrappers.FreeType
{
    /// <summary>
    /// Represents a range of characters (UTF-16) to be loaded via free type.
    /// </summary>
    public struct CharacterRange
    {

        public static readonly CharacterRange BasicLatin = new CharacterRange(0x0020, 0x007F);
        public static readonly CharacterRange Latin1Supplement = new CharacterRange(0x00A0, 0x00FF);
        public static readonly CharacterRange LatinExtendedA = new CharacterRange(0x0100, 0x017F);
        public static readonly CharacterRange LatinExtendedB = new CharacterRange(0X0180, 0x024F);

        public static readonly CharacterRange Cyrillic = new CharacterRange(0x0400, 0x04FF);
        public static readonly CharacterRange CyrillicSupplementary = new CharacterRange(0x0500, 0x052F);

        public static readonly CharacterRange CJKUnifiedIdeographs = new CharacterRange(0x4E00, 0x9FFF);

        /// <summary>
        /// Character to start at.
        /// </summary>
        public ushort from;

        /// <summary>
        /// Character to end at (including).
        /// </summary>
        public ushort to;


        public CharacterRange(ushort from, ushort to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
