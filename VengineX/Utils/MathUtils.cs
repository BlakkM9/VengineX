using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Utils
{
    public static class MathUtils
    {

        /// <summary>
        /// Overload for <see cref="CeilPoT(uint)"/>.
        /// </summary>
        public static int CeilPoT(double value) => CeilPoT((uint)value);

        /// <summary>
        /// Rounds up given value to the next power of two.
        /// </summary>
        public static int CeilPoT(uint value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return (int)++value;
        }
    }
}
