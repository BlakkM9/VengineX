namespace VengineX.ECS
{
    /// <summary>
    /// Base class for components that are attached to <see cref="Entity"/>s.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// The id of the entity this component is attached to.
        /// </summary>
        public int EntityID { get; internal set; } = 0;
    }
}
