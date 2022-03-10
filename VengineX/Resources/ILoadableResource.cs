using System;

namespace VengineX.Resources {

    /// <summary>
    /// Interface that represents a resource that can be loaded from file.<br/>
    /// Implement this if you want to load something with the <see cref="ResourceManager"/><br/>
    /// (aswell as <see cref="IDisposable"/> and <see cref="IResource"/>)
    /// </summary>
    public interface ILoadableResource
    {    
        /// <summary>
        /// Used to load this resource by <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="resourcePath">
        /// Path to resource in <see cref="ResourceManager"/>.<br/>
        /// This acts as a key within the resource manager internal dictionary.
        /// </param>
        /// <param name="loadingParameters">Parameters for loading this resource.</param>
        public void Load(string resourcePath, ref ILoadingParameters loadingParameters);
    }
}
