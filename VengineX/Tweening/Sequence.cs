namespace VengineX.Tweening
{
    /// <summary>
    /// A sequence holding different tweens which are executed one after another.
    /// </summary>
    public class Sequence : TweenBase
    {
        /// <summary>
        /// Occurs when the sequence was run to completion.
        /// </summary>
        public event Action<Sequence>? Completed;

        /// <summary>
        /// Occurs when the sequence was stopped.<br/>
        /// Also occurs when the sequence was completed.
        /// </summary>
        public event Action<Sequence>? Stopped;

        /// <summary>
        /// Iteration count for this sequence. Use <see cref="TweenBase.INFINITE"/> for infinite looping.<br/>
        /// </summary>
        public override int IterationCount { get; }

        /// <summary>
        /// The animation <see cref="Direction"/> of this sequence.<br/>
        /// When direction is not <see cref="Direction.Normal"/> this will<br/>
        /// Control and overwrites the direction of the <see cref="Tween"/>s in this sequence.<br/>
        /// It is assumed that all the tweens in this sequence are using normal direction.<br/>
        /// If you need more control over how the tweens in the sequence are looping, create multiple tweens<br/>
        /// And use <see cref="Direction.Normal"/> in the sequence.
        /// </summary>
        public override Direction Direction { get; }

        private readonly IEnumerator<(int, bool)> _sequenceIterator;

        /// <summary>
        /// List holding all the tweens in this sequence.
        /// </summary>
        private readonly List<Tween> _tweens;

        /// <summary>
        /// Index of the current tween.
        /// </summary>
        private int _currentTweenIndex;

        /// <summary>
        /// How many tweens completed in the current sequence iteration.
        /// </summary>
        private int _currentSequenceCount;

        /// <summary>
        /// Creates a sequence from given tweens.
        /// </summary>
        private Sequence(Direction direction, int iterations, params Tween[] tweens)
        {
            _tweens = tweens.ToList();

            Direction = direction;
            IterationCount = iterations;
            CurrentIterationCount = IterationCount;
            Paused = true;

            _currentTweenIndex = Reversed ? _tweens.Count - 1 : 0;
            _currentSequenceCount = 0;

            // Toggle reverse for first tween if sequence is backwards.
            if (Reversed)
            {
                foreach (Tween t in _tweens)
                {
                    t.Reverse();
                }
            }

            _sequenceIterator = SequenceIterator.GetIteratorFor(direction, _tweens.Count);
            _sequenceIterator.MoveNext();
        }


        /// <summary>
        /// Creates a new sequence with given iterations and given <see cref="Tween"/>s.
        /// </summary>
        public Sequence(int iterations, params Tween[] tweens) : this(Direction.Normal, iterations, tweens) { }

        /// <summary>
        /// Creates a new sequence with given tweens and one iteration.
        /// </summary>
        public Sequence(params Tween[] tweens) : this(Direction.Normal, 1, tweens) { }


        /// <summary>
        /// Starts/Resumes this sequence.
        /// </summary>
        public override void Start()
        {
            Paused = false;
            _tweens[_currentTweenIndex].Completed += CurrentTween_Completed;
            _tweens[_currentTweenIndex].Start();
        }


        /// <summary>
        /// Pauses this sequence.
        /// </summary>
        public override void Pause()
        {
            Paused = true;
            _tweens[_currentTweenIndex].Completed -= CurrentTween_Completed;
            _tweens[_currentTweenIndex].Pause();
        }


        /// <summary>
        /// Stops the sequence.<br/>
        /// If started again it will start at the beginning of the first sequence.
        /// </summary>
        public override void Stop()
        {
            Paused = true;
            _tweens[_currentTweenIndex].Stop();

            CurrentIterationCount = IterationCount;
            _currentTweenIndex = Reversed ? _tweens.Count - 1 : 0;

            Stopped?.Invoke(this);
        }

        private void CurrentTween_Completed(Tween currentTween)
        {
            // Remove completed listener of last tween
            currentTween.Completed -= CurrentTween_Completed;

            _currentSequenceCount++;

            // Get next index and check if running direction needs to change.
            _sequenceIterator.MoveNext();
            (int nextIndex, bool toggleReverse) result = _sequenceIterator.Current;

            // Check if still in current iteration
            if (_currentSequenceCount == _tweens.Count)
            {
                // No, start next iteration

                // Reset sequence count
                _currentSequenceCount = 0;

                // Update iteration count
                if (CurrentIterationCount != INFINITE) { CurrentIterationCount--; }

                // If no more iterations CurrentIteratorCount is 0, otherwise -1 or > 0
                if (CurrentIterationCount == 0)
                {
                    // No, finished all iterations.
                    Stop();
                    Completed?.Invoke(this);
                }
                else
                {
                    // Update index
                    _currentTweenIndex = result.nextIndex;

                    // Toggle tweens directions
                    if (result.toggleReverse)
                    {
                        Console.WriteLine("reversing all tweens");
                        foreach (Tween t in _tweens)
                        {
                            t.Reverse();
                        }
                    }

                    Start();
                }
            }
            else
            {
                // Still in iteration; go to next tween in sequence
                _currentTweenIndex = result.nextIndex;

                // Update the tweens current running direction if sequence direction is not normal
                Start();
            }
        }
    }
}
