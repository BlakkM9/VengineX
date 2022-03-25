using VengineX.Debugging.Logging;

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
        private readonly Stack<int> _availableIds;

        /// <summary>
        /// The next entity id to assign.
        /// </summary>
        private int _nextEntityId;

        /// <summary>
        /// Dictionary holding all the components.<br/>
        /// Key is the component type while value is a list of all the components.
        /// </summary>
        private readonly Dictionary<Type, List<Component>> _components;


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
        /// Attaches the given entity and all it's components to this registry.
        /// </summary>
        public E AttachEntity<E>(E entity) where E : Entity
        {
            // Assign registry and id to this entity
            entity.Registry = this;

            if (_availableIds.Count > 0)
            {
                entity.ID = _availableIds.Pop();
            }
            else
            {
                entity.ID = _nextEntityId++;
            }

            // Attach the entities components to the registry
            foreach (KeyValuePair<Type, Component> kvp in entity.EnumerateComponents())
            {
                AttachComponent(entity, kvp.Value, kvp.Key);
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
        /// Attaches the given <see cref="Component"/> to given <see cref="Entity"/>.<br/>
        /// Non generic version that needs the component type instead.
        /// </summary>
        internal Component AttachComponent(Entity entity, Component component, Type componentType)
        {
            component.EntityID = entity.ID;

            // Create list if new component type
            if (!_components.ContainsKey(componentType))
            {
                _components.Add(componentType, new List<Component>());
            }

            _components[componentType].Add(component);
            return component;
        }


        /// <summary>
        /// Removes the component of given type from given entity.
        /// </summary>
        internal void RemoveComponent<C>(Entity entity) where C : Component
        {
            C? component = entity.GetComponent<C>();

            if (component != null)
            {
                if (_components.TryGetValue(typeof(C), out List<Component>? comps))
                {
                    if (comps.Remove(component))
                    {
                        return;
                    }
                }
            }

            Logger.Log(Severity.Warning, "Component was not removed because it was not attached to this regitsry.");
        }


        /// <summary>
        /// Removes the component of given type from given entity.<br/>
        /// Non generic version that takes the component type instead.
        /// </summary>
        internal void RemoveComponent(Entity entity, Type componentType)
        {
            Component? component = entity.GetComponent(componentType);

            if (component != null)
            {
                if (_components.TryGetValue(componentType, out List<Component>? comps))
                {
                    if (comps.Remove(component))
                    {
                        return;
                    }
                }
            }

            Logger.Log(Severity.Warning, "Component was not removed because it was not attached to this regitsry.");
        }


        /// <summary>
        /// Enumerates all present <see cref="Component"/>s in this registry of given type <typeparamref name="C"/>.
        /// </summary>
        public IEnumerable<C> EnumerateComponents<C>() where C : Component
        {
            if (_components.TryGetValue(typeof(C), out List<Component>? components))
            {
                return Enumerable.Cast<C>(components);
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
