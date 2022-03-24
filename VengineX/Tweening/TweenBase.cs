namespace VengineX.Tweening
{
    /// <summary>
    /// Base class for <see cref="Tween"/> and <see cref="Sequence"/>.
    /// </summary>
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

        /// <summary>
        /// Is the tween/sequence currently paused or stopped?
        /// </summary>
        public bool Paused { get; protected set; }

        /// <summary>
        /// Is the tween/sequence direction reversed (Reverse or AlternateReverse)?
        /// </summary>
        protected bool Reversed { get => Direction == Direction.Reverse || Direction == Direction.AlternateReverse; }

        /// <summary>
        /// Is the tween/sequence direction alternating (Alternate or AlternateReverse)?
        /// </summary>
        protected bool Alternating { get => Direction == Direction.Alternate || Direction == Direction.AlternateReverse; }


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
