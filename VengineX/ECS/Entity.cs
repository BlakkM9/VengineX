namespace VengineX.ECS
{
    public abstract class Entity
    {
        /// <summary>
        /// The id of this entity
        /// </summary>
        public int ID { get; internal set; } = 0;

        /// <summary>
        /// The <see cref="ECS.Registry"/> the components of this entity life on.
        /// </summary>
        public Registry Registry { get; internal set; }

        /// <summary>
        /// Components currently attached to this entity.<br/>
        /// The components type is the key.
        /// </summary>
        private Dictionary<Type, Component> _components;


        /// <summary>
        /// Creates a new entity. Do not call this.<br/>
        /// Use <see cref="Registry.CreateEntity{E}"/> instead.
        /// </summary>
        internal Entity()
        {
            _components = new Dictionary<Type, Component>();
        }


        /// <summary>
        /// Adds a new component of given type to this entity
        /// </summary>
        /// <typeparam name="C"></typeparam>
        public void AddComponent<C>() where C : Component, new()
        {
            //Add to registry (creates the component).
            C component = Registry.AttachComponent<C>(this);

            // Add to self.
            _components.Add(typeof(C), component);
        }


        /// <summary>
        /// Adds the given component to this entity
        /// </summary>
        /// <typeparam name="C"></typeparam>
        public void AddComponent<C>(C component) where C : Component
        {
            //Add to registry (creates the component).
            Registry.AttachComponent<C>(this, component);

            // Add to self.
            _components.Add(typeof(C), component);
        }


        /// <summary>
        /// Gets the component of given type in this entity.<br/>
        /// Returns null if the component is not present.
        /// </summary>
        public C? GetComponent<C>() where C : Component
        {
            if (_components.TryGetValue(typeof(C), out Component? component))
            {
                return (C)component;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Checks if a component of given type is attached.
        /// </summary>
        /// <returns>true if attached, false otherwise.</returns>
        public bool HasComponent<C>() where C : Component
        {
            return _components.ContainsKey(typeof(C));
        }


        public void RemoveComponent<C>() where C : Component
        {
            // Remove from registry
            Registry.RemoveComponent<C>(this);

            // Remove from self
            _components.Remove(typeof(C));
        }


        public IEnumerable<KeyValuePair<Type, Component>> EnumerateComponents()
        {
            return _components.AsEnumerable();
        }
    }
}
