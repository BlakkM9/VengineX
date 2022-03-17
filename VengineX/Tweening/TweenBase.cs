using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Tweening
{
    public abstract class TweenBase
    {
        /// <summary>
        /// Constant for infitite iterations.
        /// </summary>
        public const int INFINITE = -1;

        /// <summary>
        /// Iteration count of this animation.<br/>
        /// Any value smaller 0 means infinite iterations. Use <see cref="INFINITE"/> to set it.
        /// </summary>
        public virtual int IterationCount { get; }

        /// <summary>
        /// How many iterations are currently left in this animation.
        /// </summary>
        public virtual int CurrentIterationCount { get; protected set; }

        /// <summary>
        /// The animation <see cref="Direction"/> of this tween/sequence.
        /// </summary>
        public abstract Direction Direction { get; }

        protected bool IsReverse { get => Direction == Direction.Reverse || Direction == Direction.AlternateReverse; }

        protected bool IsAlternating { get => Direction == Direction.Alternate || Direction == Direction.AlternateReverse; }


        /// <summary>
        /// Starts the animation.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public abstract void Stop();
    }
}
