namespace VengineX.ECS
{
    /// <summary>
    /// This class handles all the current entites and components.<br/>
    /// Use to create and remove entities.
    /// </summary>
    public class Registry
    {
        /// <summary>
        /// IDs that became available because entities were removed.
        /// </summary>
        private Stack<int> _availableIds;

        /// <summary>
        /// The next entity id to assign.
        /// </summary>
        private int _nextEntityId;

        /// <summary>
        /// Dictionary holding all the components.<br/>
        /// Key is the component type while value is a list of all the components.
        /// </summary>
        private Dictionary<Type, List<Component>> _components;


        /// <summary>
        /// Creates a new empty registry.
        /// </summary>
        public Registry()
        {
            _availableIds = new Stack<int>();
            _nextEntityId = 1;
            _components = new Dictionary<Type, List<Component>>();
        }


        /// <summary>
        /// Creates a new <see cref="Entity"/> of given type <typeparamref name="E"/>.
        /// </summary>
        public E CreateEntity<E>() where E : Entity, new()
        {
            E entity = new E();
            entity.Registry = this;

            if (_availableIds.Count > 0)
            {
                entity.ID = _availableIds.Pop();
            }
            else
            {
                entity.ID = _nextEntityId++;
            }

            return entity;
        }


        /// <summary>
        /// Removes this entity from the registry (and all its components).
        /// </summary>
        public void RemoveEntity(Entity entity)
        {
            int id = entity.ID;
            entity.ID = 0;
            entity.Registry = null;
            _availableIds.Push(id);


            foreach (KeyValuePair<Type, Component> kvp in entity.EnumerateComponents())
            {
                kvp.Value.EntityID = 0;
                _components[kvp.Key].Remove(kvp.Value);
            }
        }


        /// <summary>
        /// Creates and attaches a <see cref="Component"/> of given type <typeparamref name="C"/> to given <see cref="Entity"/>.
        /// </summary>
        internal C AttachComponent<C>(Entity entity) where C : Component, new()
        {
            C component = new C();
            return AttachComponent(entity, component);
        }


        /// <summary>
        /// Attaches the given <see cref="Component"/> to given <see cref="Entity"/>.
        /// </summary>
        internal C AttachComponent<C>(Entity entity, C component) where C : Component
        {
            component.EntityID = entity.ID;

            // Create list if new component type
            if (!_components.ContainsKey(typeof(C)))
            {
                _components.Add(typeof(C), new List<Component>());
            }

            _components[typeof(C)].Add(component);
            return component;
        }


        /// <summary>
        /// Removes the component of given type from given entity.
        /// </summary>
        internal void RemoveComponent<C>(Entity entity) where C : Component
        {
            C component = entity.GetComponent<C>();
            _components[typeof(C)].Remove(component);
        }


        /// <summary>
        /// Enumerates all present <see cref="Component"/>s in this registry of given type <typeparamref name="C"/>.
        /// </summary>
        internal IEnumerable<C> EnumerateComponents<C>() where C : Component
        {
            if (_components.TryGetValue(typeof(C), out List<Component>? components))
            {
                return (IEnumerable<C>)components;
            }
            else
            {
                return Enumerable.Empty<C>();
            }
        }


        /// <summary>
        /// Clears all <see cref="Component"/>s from this regitry
        /// </summary>
        public void Clear()
        {
            foreach (List<Component> comp in _components.Values)
            {
                comp.Clear();
            }

            _components.Clear();
        }
    }
}
