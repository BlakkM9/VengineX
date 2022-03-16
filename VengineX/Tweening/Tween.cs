using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;
using VengineX.Utils.Extensions;

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
        public event Action<Tween>? Finished;

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
        /// When true, <see cref="Finished"/> will never occur.
        /// </summary>
        public bool Loop { get; set; }

        private EasingFunction _easingFunction;

        private UpdateFunction _updateFunction;


        public Tween(float duration, EasingFunction easingFunction, UpdateFunction updateFunction)
        {
            Duration = duration;
            _easingFunction = easingFunction;
            _updateFunction = updateFunction;
        }


        public Tween(float duration, Tweening.EasingFunction easingFunction, UpdateFunction updateFunction)
            : this(duration, easingFunction.Function(), updateFunction) { }


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
                    Finished?.Invoke(this);
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
        }
    }
}
