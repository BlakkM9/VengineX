using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Utils.Easing;
using VengineX.Utils.Extensions;
using static VengineX.Utils.Easing.EasingFunctions;

namespace VengineX.Tweening
{
    /// <summary>
    /// Class that provides an easy solution for creating animations.
    /// </summary>
    public class Tween : TweenBase
    {
        /// <summary>
        /// Occurs when the tween was run to completion.
        /// </summary>
        public event Action<Tween>? Completed;

        /// <summary>
        /// Occurs when the tween was stopped.<br/>
        /// Also occurs when tween was completed.
        /// </summary>
        public event Action<Tween>? Stopped;

        /// <summary>
        /// Function that calculates <see cref="T"/> for the update function<br/>
        /// based on the duration and current time of the tween.
        /// </summary>
        public delegate float EasingFunction(float duration, float currentTime);

        /// <summary>
        /// This function is called every frame update with the current value of <see cref="T"/>,
        /// which was calculated before by the <see cref="EasingFunction"/>
        /// </summary>
        /// <param name="t"></param>
        public delegate void UpdateFunction(float t);

        /// <summary>
        /// The duration the tween is supposed to last (in seconds).
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// The current time normalized time of this tween.<br/>
        /// This means 0 is tween start and 1 tween end (with linear easing).
        /// </summary>
        public float T { get; private set; }

        /// <summary>
        /// The time the tween has run (in seconds).
        /// </summary>
        public float CurrentTime { get; private set; }

        /// <summary>
        /// The iteration count of this tween.<br/>
        /// Set to <see cref="TweenBase"/> (-1) for infinite iterations.<br/>
        /// When this tween is part of a <see cref="Sequence"/>, setting this to <see cref="TweenBase.INFINITE"/> will<br/>
        /// break the sequence and keep looping this tween.<br/>
        /// When <see cref="TweenBase.INFINITE"/>, <see cref="Completed"/> will never occur.
        /// </summary>
        public override int IterationCount { get; }

        /// <summary>
        /// The animation direction of this tween.
        /// </summary>
        public override Direction Direction { get; }

        /// <summary>
        /// Is this tween currently running reversed?
        /// </summary>
        public bool IsRunningReversed { get; private set; }


        private readonly EasingFunction _easingFunction;
        private readonly UpdateFunction _updateFunction;


        /// <summary>
        /// Most basic constructor for a tween. Own defined easing and and update functions.
        /// </summary>
        /// <param name="duration">Duration of the tween in seconds.</param>
        /// <param name="direction">Animation direction of this tween.</param>
        /// <param name="iterations">How often this tween should be played. Use <see cref="TweenBase.INFINITE"/> (-1) for infinite iterations.</param>
        /// <param name="easingFunction"><see cref="EasingFunction"/> for this tween.</param>
        /// <param name="updateFunction"><see cref="UpdateFunction"/> function for this tween.</param>
        public Tween(float duration, Direction direction, int iterations, EasingFunction easingFunction, UpdateFunction updateFunction)
        {
            Duration = duration;

            Direction = direction;
            IsRunningReversed = IsReverse;

            IterationCount = iterations;
            CurrentIterationCount = IterationCount;

            _easingFunction = easingFunction;
            _updateFunction = updateFunction;
        }


        /// <summary>
        /// Overload for <see cref="Tween(float, Direction, int, EasingFunction, UpdateFunction)"/>, using<br/>
        /// <see cref="Tweening.EasingFunction"/> enum for easing function.
        /// </summary>
        public Tween(float duration, Direction direction, int iterations, Tweening.EasingFunction easingFunction, UpdateFunction updateFunction)
            : this(duration, direction, iterations, easingFunction.Function(), updateFunction) { }


        /// <summary>
        /// Overload for <see cref="Tween(float, Direction, int, EasingFunction, UpdateFunction)"/>, using<br/>
        /// css-like cubic-bezier (<see cref="CubicBezier"/>) for the easing function.<br/>
        /// If there is a pre-defined <see cref="Tweening.EasingFunction"/>, use this one<br/>
        /// instead for (usually) better performance.
        /// </summary>
        public Tween(float duration, Direction direction, int iterations, CubicBezier easingFunction, UpdateFunction updateFunction)
            : this(duration, direction, iterations, (d, c) => (float)easingFunction.Solve(c / d, 1e-6), updateFunction) { }


        public Tween(float duration, EasingFunction easingFunction, UpdateFunction updateFunction)
            : this(duration, Direction.Normal, 1, easingFunction, updateFunction) { }


        public Tween(float duration, Tweening.EasingFunction easingFunction, UpdateFunction updateFunction)
            : this(duration, Direction.Normal, 1, easingFunction, updateFunction) { }


        public Tween(float duration, CubicBezier easingFunction, UpdateFunction updateFunction)
            : this(duration, Direction.Normal, 1, easingFunction, updateFunction) { }



        /// <summary>
        /// Updates the current value of <see cref="T"/>,<br/>
        /// calls the update function and checks if the tween is over/loops the tween.
        /// </summary>
        internal void Update()
        {
            CurrentTime += (float)Time.DeltaUpdate;
            T = _easingFunction(Duration, CurrentTime);


            if (IsRunningReversed)
            {
                _updateFunction.Invoke(1 - T);
            }
            else
            {
                _updateFunction.Invoke(T);
            }
            

            // Check if tween interation is over
            if (CurrentTime > Duration)
            {
                // Update current iteration count if not infinite
                if (IterationCount != INFINITE) { CurrentIterationCount--; }

                // Check if more iterations or stopping
                if (IterationCount == INFINITE || CurrentIterationCount > 0)
                {
                    CurrentTime = 0;
                    T = 0;

                    // Update alternation
                    if (IsAlternating)
                    {
                        Reverse();
                    }
                }
                else if (CurrentIterationCount == 0)
                {
                    Stop();
                    Completed?.Invoke(this);
                }
            }
        }


        /// <summary>
        /// Starts/Resumes this tween.
        /// </summary>
        public override void Start() => TweenManager.AddTween(this);


        /// <summary>
        /// Pauses this tween.
        /// </summary>
        public override void Pause() => TweenManager.RemoveTween(this);


        /// <summary>
        /// Stops this tween.<br/>
        /// If the tween is started again, it will start at the beginning again.
        /// </summary>
        public override void Stop()
        {
            TweenManager.RemoveTween(this);
            T = 0;
            CurrentTime = 0;
            CurrentIterationCount = IterationCount;
            Stopped?.Invoke(this);
        }


        /// <summary>
        /// Toggles <see cref="IsRunningReversed"/>.
        /// </summary>
        public void Reverse()
        {
            IsRunningReversed = !IsRunningReversed;
        }

        /// <summary>
        /// Returns a tween that can servers as a delay (between tweens within a sequence)
        /// </summary>
        /// <param name="duration">Delay in seconds.</param>
        public static Tween Delay(float duration) => new Tween(duration, Tweening.EasingFunction.None, (_) => { });
    }
}
