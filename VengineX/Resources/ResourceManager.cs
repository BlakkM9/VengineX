using System;
using System.Collections.Generic;
using VengineX.Debugging.Logging;

namespace VengineX.Resources
{
    /// <summary>
    /// Caches and manages all the resources for the game.
    /// </summary>
    public static class ResourceManager 
    {

        private static readonly Dictionary<Type, Dictionary<string, IDisposable>> _resourceMap = new Dictionary<Type, Dictionary<string, IDisposable>>();


        /// <summary>
        /// Puts a resource in cache. Use for resources that are created at runtime.
        /// </summary>
        /// <param name="resourcePath">Path to the resource (not file path but a path that is used as a key)</param>
        /// <param name="resource">Resource to put in cache</param>
        public static void CacheResource<T>(string resourcePath, T resource) where T : IDisposable, IResource
        {
            Type type = typeof(T);

            // Add to resource dictionary
            if (!_resourceMap.ContainsKey(type))
            {
                // New resource type, create new dict for it
                _resourceMap.Add(type, new Dictionary<string, IDisposable>());
            }

            // Add resource to correct dict
            if (_resourceMap[type].TryAdd(resourcePath, resource))
            {
                // Save resource path in resource.
                resource.ResourcePath = resourcePath;
                Logger.Log(Severity.Info, Tag.Loading, "Cached " + resourcePath + " (" + type.Name + ")");
            }
            else
            {
                Logger.Log(Severity.Error, Tag.Loading, "Failed to cache " + resourcePath + " (" + type.Name + "): it is already in cache!");
            }
        }


        /// <summary>
        /// Loads a resource into resource cache.
        /// </summary>
        /// <param name="resourcePath">Path to the resource (not file path but a path that is used as a key)</param>
        /// <param name="loadingParameters"><see cref="ILoadingParameters"/></param>
        /// <returns>Loaded resource</returns>
        public static T LoadResource<T>(string resourcePath, ILoadingParameters loadingParameters)
            where T : ILoadableResource, IDisposable, IResource, new()
        {
            // Instantiate resource
            T resource = new();

            // Load resource
            resource.Load(ref loadingParameters);
            Type type = typeof(T);

            // Add to resource dictionary
            if (!_resourceMap.ContainsKey(type))
            {
                // New resource type, create new dict for it
                _resourceMap.Add(type, new Dictionary<string, IDisposable>());
            }

            // Add resource to correct dict
            if (_resourceMap[type].TryAdd(resourcePath, resource))
            {
                // Save resource path in resource.
                resource.ResourcePath = resourcePath;
                Logger.Log(Severity.Info, Tag.Loading, "Loaded " + resourcePath + " (" + type.Name + ")");
                return resource;
            }
            else
            {
                Logger.Log(Severity.Error, Tag.Loading, "Failed to load " + resourcePath + " (" + type.Name + "): it is already loaded!");
                return GetResource<T>(resourcePath);
            }
        }


        /// <summary>
        /// Gets the resource at given resource path.
        /// </summary>
        /// <typeparam name="T">Type of the requested resource.</typeparam>
        /// <param name="resourcePath">ResourcePath to the requrested resource.</param>
        /// <returns>Requested resource. Fatal error if resource not found.</returns>
        public static T GetResource<T>(string resourcePath) where T : IDisposable, IResource, new()
        {
            Type type = typeof(T);

            if (_resourceMap.ContainsKey(type))
            {
                if (_resourceMap[type].TryGetValue(resourcePath, out var resource))
                {
                    return (T)resource;
                }
            }

            Logger.Log(Severity.Fatal, Tag.Loading, "Failed to get resource from cache: " + resourcePath + " (" + type.Name +")");
            return default;
        }


        /// <summary>
        /// Unloads (disposes) the resource of given type at given resource path (if existing).
        /// </summary>
        /// <typeparam name="T">Type of the resource to unload.</typeparam>
        /// <param name="resourcePath">ResourcePath of the resource.</param>
        public static void UnloadResource<T>(string resourcePath) where T : IDisposable, IResource, new()
        {
            Type type = typeof(T);

            if (_resourceMap.ContainsKey(type))
            {
                if (_resourceMap[type].TryGetValue(resourcePath, out var resource))
                {
                    resource.Dispose();
                    _resourceMap[type].Remove(resourcePath);
                    return;
                }
            }

            Logger.Log(Severity.Fatal, Tag.Loading, "Failed to unload resource from cache: " + resourcePath + " (" + type.Name + ")");
        }


        /// <summary>
        /// Overload of <see cref="UnloadResource{T}(string)"/> for easier use with <see cref="ILoadableResource"/>s.<br/>
        /// Simply takes the resource itself as parameter.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the resource.<br/>
        /// Usually not required as it is determined by the type of <paramref name="resource"/> passed.
        /// </typeparam>
        /// <param name="resource">Resource to unload.</param>
        public static void UnloadResource<T>(T resource) where T : IDisposable, ILoadableResource, IResource, new()
        {
            UnloadResource<T>(resource.ResourcePath);
        }


        /// <summary>
        /// Unloads all resources that are currently cached inside the resource manager.
        /// </summary>
        public static void UnloadAllResources()
        {
            foreach (KeyValuePair<Type, Dictionary<string, IDisposable>> outerEntry in _resourceMap)
            {
                foreach (KeyValuePair<string, IDisposable> innerEntry in outerEntry.Value)
                {
                    Logger.Log($"Unloading resource that was not disposed in UnloadAllResources: {innerEntry.Key} ({outerEntry.Key.Name})");
                    innerEntry.Value.Dispose();
                }
            }

            _resourceMap.Clear();
        }
    }
}
