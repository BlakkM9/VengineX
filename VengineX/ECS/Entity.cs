using VengineX.Debugging.Logging;

namespace VengineX.ECS
{
    /// <summary>
    /// Base class for all entites in the ECS.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// The id of this entity
        /// </summary>
        public int ID { get; internal set; } = 0;

        /// <summary>
        /// The <see cref="ECS.Registry"/> the components of this entity life on.
        /// </summary>
        public Registry? Registry { get; internal set; }

        /// <summary>
        /// Components currently attached to this entity.<br/>
        /// The components type is the key.
        /// </summary>
        private readonly Dictionary<Type, Component> _components;


        /// <summary>
        /// Creates a new entity that is not attached to any registry.
        /// </summary>
        public Entity()
        {
            _components = new Dictionary<Type, Component>();
        }


        /// <summary>
        /// Adds the given component to this entity
        /// </summary>
        /// <typeparam name="C"></typeparam>
        public void AddComponent<C>(C component) where C : Component
        {
            //Add to registry (creates the component).
            Registry?.AttachComponent(this, component);

            // Add to self.
            _components.Add(typeof(C), component);
            component.Entity = this;
            component.Attached();
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
        /// Gets the component of given type in this entity.<br/>
        /// Non generic version, taking component type instead.<br/>
        /// Returns null if the component is not present.
        /// </summary>
        public Component? GetComponent(Type componentType)
        {
            if (_components.TryGetValue(componentType, out Component? component))
            {
                return component;
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


        /// <summary>
        /// Removes the given component from the registry and from self.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        public void RemoveComponent<C>() where C : Component
        {
            // Remove from registry
            Registry?.RemoveComponent<C>(this);

            // Remove from self
            Component comp = GetComponent<C>();
            _components.Remove(typeof(C));
            comp.Entity = null;
            comp.Detached();

        }


        /// <summary>
        /// Enumerates over all the components in this enitity
        /// </summary>
        public IEnumerable<KeyValuePair<Type, Component>> EnumerateComponents()
        {
            return _components.AsEnumerable();
        }


        /// <summary>
        /// Called when this entity is attached to the registry.
        /// </summary>
        public virtual void Registered() { }

        /// <summary>
        /// Called when this entity is detachted from the registry.
        /// </summary>
        public virtual void Unregistered() { }
    }
}
