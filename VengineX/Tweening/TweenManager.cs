using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Tweening
{
    /// <summary>
    /// Class that manages all the <see cref="Tween"/>s / tweening.
    /// </summary>
    internal static class TweenManager
    {
        /// <summary>
        /// List holding all the tweens that are currently active.
        /// </summary>
        private readonly static List<Tween> _tweens = new List<Tween>();

        /// <summary>
        /// Queue holding all the tweens that were started while updating current tweens.<br/>
        /// They will be added once the updating in this frame is done.
        /// </summary>
        private readonly static Queue<Tween> _addQueue = new Queue<Tween>();

        /// <summary>
        /// Queue holding all the tweens that were stopped (removed) while updating current tweens.<br/>
        /// They will be added once the updating in this frame is done.
        /// </summary>
        private readonly static Queue<Tween> _removeQueue = new Queue<Tween>();


        /// <summary>
        /// Updates all <see cref="_tweens"/> (active tweens).
        /// </summary>
        internal static void Update()
        {
            // Update currently active tweens.
            foreach (Tween tween in _tweens)
            {
                tween.Update();
            }

            // Remove tweens that were stopped.
            while (_removeQueue.Count > 0)
            {
                _tweens.Remove(_removeQueue.Dequeue());
            }

            // Add tweens that were started.
            while (_addQueue.Count > 0)
            {
                _tweens.Add(_addQueue.Dequeue());
            }
        }


        /// <summary>
        /// Queues a tween for adding.
        /// </summary>
        /// <param name="tween">Tween to add.</param>
        internal static void AddTween(Tween tween)
        {
            _addQueue.Enqueue(tween);
        }


        /// <summary>
        /// Queues a tween for removing.
        /// </summary>
        /// <param name="tween">Tween to remove.</param>
        internal static void RemoveTween(Tween tween)
        {
            _removeQueue.Enqueue(tween);
        }
    }
}
