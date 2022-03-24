﻿namespace VengineX.ECS
{
    public abstract class Component
    {
        /// <summary>
        /// The id of the entity this component is attached to.
        /// </summary>
        public int EntityID { get; internal set; } = 0;
    }
}
