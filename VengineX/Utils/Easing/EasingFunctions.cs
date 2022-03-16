using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Utils.Easing
{
    public static class EasingFunctions
    {
        private const float BOUNCE_N1 = 7.5625f;
        private const float BOUNCE_D1 = 2.75f;

        #region Shortcuts

        private const float PI = MathHelper.Pi;
        private static float Cos(float x) => (float)MathHelper.Cos(x);
        private static float Sin(float x) => (float)MathHelper.Sin(x);
        private static float Pow(float x, float y) => (float)MathHelper.Pow(x, y);

        #endregion

        public static float EaseInSine(float x) => 1 - Cos(x * PI / 2);
        public static float EaseOutSine(float x) => Sin(x * PI / 2);
        public static float EaseInOutSine(float x) => -(Cos(x * PI) - 1) / 2;
        public static float EaseInCubic(float x) => x * x * x;
        public static float EaseOutCubic(float x) => 1 - Pow(1 - x, 3);
        public static float EaseInOutCubic(float x) => x < 0.5f ? 4 * x * x * x : 1 - Pow(-2 * x + 2, 3) / 2;

        // ...

        public static float EaseInBounce(float x) => 1 - EaseOutBounce(1 - x);
        public static float EaseOutBounce(float x)
        {
            if (x < 1f / BOUNCE_D1)
            {
                return BOUNCE_N1 * x * x;
            }
            else if (x < 2f / BOUNCE_D1)
            {
                return BOUNCE_N1 * (x -= 1.5f / BOUNCE_D1) * x + 0.75f;
            }
            else if (x < 2.5f / BOUNCE_D1)
            {
                return BOUNCE_N1 * (x -= 2.25f / BOUNCE_D1) * x + 0.9375f;
            }
            else
            {
                return BOUNCE_N1 * (x -= 2.625f / BOUNCE_D1) * x + 0.984375f;
            }
        }
        public static float EaseInOutBounce(float x) => x < 0.5 ? (1 - EaseOutBounce(1 - 2 * x)) / 2 : (1 + EaseOutBounce(2 * x - 1)) / 2;
    }
}
