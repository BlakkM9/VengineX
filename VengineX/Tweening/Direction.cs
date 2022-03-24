namespace VengineX.Tweening
{
    /// <summary>
    /// The animation direction of a <see cref="Tween"/>s or <see cref="Sequence"/>s.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Animation is played normal (forwards). Default.
        /// </summary>
        Normal,

        /// <summary>
        /// Animation is played in reversed direction (backwards)
        /// </summary>
        Reverse,

        /// <summary>
        /// Animation is played forward first, then backwards.
        /// </summary>
        Alternate,

        /// <summary>
        /// Animation is played backwards first, then forwards.
        /// </summary>
        AlternateReverse
    }
}
