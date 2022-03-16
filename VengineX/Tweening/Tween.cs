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
    public class Tween
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
        /// Wether or not this tween should loop.<br/>
        /// When this tween is part of a <see cref="Sequence"/>, setting this to true will<br/>
        /// break the sequence and keep looping the tween with loop set to true.<br/>
        /// When true, <see cref="Completed"/> will never occur.
        /// </summary>
        public bool Loop { get; set; }

        private EasingFunction _easingFunction;

        private UpdateFunction _updateFunction;


        /// <summary>
        /// Most basic constructor for a tween. Own defined easing and and update functions.
        /// </summary>
        /// <param name="duration">Duration of the tween in seconds.</param>
        public Tween(float duration, EasingFunction easingFunction, UpdateFunction updateFunction)
        {
            Duration = duration;
            _easingFunction = easingFunction;
            _updateFunction = updateFunction;
        }


        /// <summary>
        /// Overload for <see cref="Tween(float, EasingFunction, UpdateFunction)"/>, using<br/>
        /// <see cref="Tweening.EasingFunction"/> enum for easing function.
        /// </summary>
        public Tween(float duration, Tweening.EasingFunction easingFunction, UpdateFunction updateFunction)
            : this(duration, easingFunction.Function(), updateFunction) { }


        /// <summary>
        /// Overload for <see cref="Tween(float, EasingFunction, UpdateFunction)"/>, using<br/>
        /// css-like cubic-bezier (<see cref="UnitBezier"/>) for the easing function.<br/>
        /// If there is a pre-defined <see cref="Tweening.EasingFunction"/>, use this one<br/>
        /// instead for (usually) better performance.
        /// </summary>
        public Tween(float duration, UnitBezier cubicBezier, UpdateFunction updateFunction)
            : this(duration, (d, c) => (float)cubicBezier.Solve(c / d, 1e-6), updateFunction) { }


        /// <summary>
        /// Updates the current value of <see cref="T"/>,<br/>
        /// calls the update function and checks if the tween is over/loops the tween.
        /// </summary>
        internal void Update()
        {
            CurrentTime += (float)Time.DeltaUpdate;
            T = _easingFunction(Duration, CurrentTime);
            _updateFunction.Invoke(T);

            if (CurrentTime > Duration)
            {
                if (Loop)
                {
                    CurrentTime = 0;
                    T = 0;
                }
                else
                {
                    Stop();
                    Completed?.Invoke(this);
                }
            }
        }


        /// <summary>
        /// Starts/Resumes this tween.
        /// </summary>
        public void Start()
        {
            TweenManager.AddTween(this);
        }


        /// <summary>
        /// Pauses this tween.
        /// </summary>
        public void Pause()
        {
            TweenManager.RemoveTween(this);
        }


        /// <summary>
        /// Stops this tween.<br/>
        /// If the tween is started again, it will start at the beginning again.
        /// </summary>
        public void Stop()
        {
            TweenManager.RemoveTween(this);
            T = 0;
            CurrentTime = 0;
            Stopped?.Invoke(this);
        }


        /// <summary>
        /// Returns a tween that can servers as a delay (between tweens within a sequence)
        /// </summary>
        /// <param name="duration">Delay in seconds.</param>
        public static Tween Delay(float duration)
        {
            return new Tween(duration, Tweening.EasingFunction.Linear, (_) => { });
        }
    }
}
