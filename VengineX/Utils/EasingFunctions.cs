using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Utils
{
    public static class EasingFunctions
    {
        #region Shortcuts

        private const float PI = MathHelper.Pi;
        private static float Cos(float x) => (float)MathHelper.Cos(x);
        private static float Sin(float x) => (float)MathHelper.Sin(x);
        private static float Pow(float x, float y) => (float)MathHelper.Pow(x, y);

        #endregion

        public static float EaseInSine(float x) => 1 - Cos((x * PI) / 2);
        public static float EaseOutSine(float x) => Sin((x * PI) / 2);
        public static float EaseInOutSine(float x) => -(Cos(x * PI) - 1) / 2;
        public static float EaseInCubic(float x) => x * x * x;
        public static float EaseOutCubic(float x) => 1 - Pow(1 - x, 3);
        public static float EaseInOutCubic(float x) => x < 0.5f ? 4 * x * x * x : 1 - Pow(-2 * x + 2, 3) / 2;
    }
}
