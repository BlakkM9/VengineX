using VengineX.Tweening;
using VengineX.Utils.Easing;

namespace VengineX.Utils.Extensions
{
    /// <summary>
    /// Extensions for <see cref="EasingFunction"/> enum.
    /// </summary>
    public static class EasingFunctionExtensions
    {
        /// <summary>
        /// Returns the <see cref="Tween.EasingFunction"/> for the given <see cref="EasingFunction"/>.
        /// </summary>
        public static Tween.EasingFunction Function(this EasingFunction easingFunction)
        {
            return easingFunction switch
            {
                EasingFunction.None => (d, c) => 0,
                EasingFunction.Linear => (d, c) => c / d,
                EasingFunction.EaseInSine => (d, c) => EasingFunctions.EaseInSine(c / d),
                EasingFunction.EaseOutSine => (d, c) => EasingFunctions.EaseOutSine(c / d),
                EasingFunction.EaseInOutSine => (d, c) => EasingFunctions.EaseInOutSine(c / d),
                EasingFunction.EaseInCubic => (d, c) => EasingFunctions.EaseInCubic(c / d),
                EasingFunction.EaseOutCubic => (d, c) => EasingFunctions.EaseOutCubic(c / d),
                EasingFunction.EaseInOutCubic => (d, c) => EasingFunctions.EaseInOutCubic(c / d),

                // ...

                EasingFunction.EaseInBounce => (d, c) => EasingFunctions.EaseInBounce(c / d),
                EasingFunction.EaseOutBounce => (d, c) => EasingFunctions.EaseOutBounce(c / d),
                EasingFunction.EaseInOutBounce => (d, c) => EasingFunctions.EaseInOutBounce(c / d),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
