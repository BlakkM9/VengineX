using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Tweening
{
    public enum EasingFunction
    {
        None,
        Linear,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,

        // ...

        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
    }
}
