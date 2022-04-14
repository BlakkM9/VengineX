namespace VengineX.ECS
{
    /// <summary>
    /// Base class for components that are attached to <see cref="Entity"/>s.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// As what type to add this component in the registry.
        /// </summary>
        internal Type RegistryType { get; }

        /// <summary>
        /// The id of the entity this component is attached to.
        /// </summary>
        public int EntityID { get; internal set; } = 0;

        /// <summary>
        /// The entity this component is attached to.<br/>
        /// The entity does not need to be registered for that value to be set.<br/>
        /// Null if not attached to any entity.
        /// </summary>
        public Entity? Entity { get; internal set; }

        /// <summary>
        /// Sets or gets wether the component is currently enabled or not.
        /// </summary>
        public bool Enabled { get; set; } = true;


        /// <summary>
        /// Creates a new component that is registered as the given <paramref name="registryType"/><br/>
        /// When attached to a <see cref="Registry"/>.
        /// </summary>
        public Component(Type registryType)
        {
            RegistryType = registryType;
        }


        /// <summary>
        /// Called when this component is attached to an entity.<br/>
        /// The entity does not have to be registered at that point.
        /// </summary>
        public virtual void Attached() { }


        /// <summary>
        /// Called when this component is detached from an entity.<br/>
        /// The entity does not have to be registered at that point.
        /// </summary>
        public virtual void Detached() { }


        /// <summary>
        /// Called when this component is attached to the registry.
        /// </summary>
        public virtual void Registered() { }


        /// <summary>
        /// Called when this component is detached from the registry.
        /// </summary>
        public virtual void Unregistered() { }
    }
}
