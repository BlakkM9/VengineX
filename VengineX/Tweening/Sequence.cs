using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Tweening
{
    /// <summary>
    /// A sequence holding different tweens which are executed one after another.
    /// </summary>
    public class Sequence
    {
        /// <summary>
        /// Occurs when the sequence was run to completion.
        /// </summary>
        public event Action<Sequence>? Finished;

        /// <summary>
        /// Wether or not this sequence should loop.
        /// </summary>
        public bool Loop { get; set; }

        private Tween CurrentTween { get => _tweens[_currentTweenIndex]; }

        private readonly List<Tween> _tweens;

        private int _currentTweenIndex;


        private Sequence()
        {
            _tweens = new List<Tween>();
            _currentTweenIndex = 0;
        }


        /// <summary>
        /// Creates a sequence from given tweens.
        /// </summary>
        public Sequence(params Tween[] tweens)
        {
            _tweens = tweens.ToList();
        }


        /// <summary>
        /// Starts/Resumes this sequence.
        /// </summary>
        public void Start()
        {
            CurrentTween.Finished += CurrentTween_Over;
            CurrentTween.Start();
        }


        /// <summary>
        /// Pauses this sequence.
        /// </summary>
        public void Pause()
        {
            CurrentTween.Finished -= CurrentTween_Over;
            CurrentTween.Pause();
        }


        /// <summary>
        /// Stops the sequence.<br/>
        /// If started again it will start at the beginning of the first sequence.
        /// </summary>
        public void Stop()
        {
            CurrentTween.Stop();
            _currentTweenIndex = 0;
        }


        private void CurrentTween_Over(Tween currentTween)
        {
            currentTween.Finished -= CurrentTween_Over;
            _currentTweenIndex++;

            if (_currentTweenIndex < _tweens.Count)
            {
                CurrentTween.Finished += CurrentTween_Over;
                CurrentTween.Start();
            }
            else
            {
                if (Loop)
                {
                    _currentTweenIndex = 0;
                    Start();
                }
                else
                {
                    _currentTweenIndex = 0;
                    Finished?.Invoke(this);
                }
            }
        }
    }
}
