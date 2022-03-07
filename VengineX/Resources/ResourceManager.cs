using System;
using System.Collections.Generic;
using VengineX.Debugging.Logging;

namespace VengineX.Resources
{
    public static class ResourceManager 
    {

        private static readonly Dictionary<Type, Dictionary<string, IDisposable>> _resourceMap = new Dictionary<Type, Dictionary<string, IDisposable>>();


        /// <summary>
        /// Puts a resource in cache. Use for resources that are created at runtime.
        /// </summary>
        /// <param name="resourcePath">Path to the resource (not file path but a path that is used as a key)</param>
        /// <param name="resource">Resource to put in cache</param>
        public static void CacheResource<T>(string resourcePath, T resource) where T : IDisposable
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
        /// <param name="fileType"><see cref="ILoadableResource.Load(string, string, ref ILoadingParameters)"/></param>
        /// <param name="loadingParameters"><see cref="ILoadingParameters"/></param>
        /// <returns>Loaded resource</returns>
        public static T? LoadResource<T>(string resourcePath, string fileType, ILoadingParameters loadingParameters) where T : ILoadableResource, IDisposable, new()
        {

            // Instantiate resource
            T resource = new();

            // Load resource
            resource.Load(resourcePath, fileType, ref loadingParameters);
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
                Logger.Log(Severity.Info, Tag.Loading, "Loaded " + resourcePath + " (" + type.Name + ")");
                return resource;
            }
            else
            {
                Logger.Log(Severity.Error, Tag.Loading, "Failed to load " + resourcePath + " (" + type.Name + "): it is already loaded!");
                return GetResource<T>(resourcePath);
            }
        }


        public static T? GetResource<T>(string resourcePath) where T : IDisposable, new()
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

        public static void UnloadResource<T>(string resourcePath) where T : IDisposable, new()
        {
            Type type = typeof(T);

            if (_resourceMap.ContainsKey(type))
            {
                if (_resourceMap[type].TryGetValue(resourcePath, out var resource))
                {
                    resource.Dispose();
                    return;
                }
            }

            Logger.Log(Severity.Fatal, Tag.Loading, "Failed to unload resource from cache: " + resourcePath + " (" + type.Name + ")");
        }

        public static void UnloadAllResources()
        {
            foreach (KeyValuePair<Type, Dictionary<string, IDisposable>> outerEntry in _resourceMap)
            {
                foreach (KeyValuePair<string, IDisposable> innerEntry in outerEntry.Value)
                {
                    // We can safely cast to IDisposable because we made sure it implements it
                    innerEntry.Value.Dispose();
                }
            }

            _resourceMap.Clear();
        }
    }
}
