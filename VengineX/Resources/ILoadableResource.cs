using System;

namespace VengineX.Resources {

    /// <summary>
    /// Interface that represents a resource that can be loaded from files.
    /// Implement this if you want to load something with the <see cref="ResourceManager"/> (aswell as <see cref="IDisposable"/>)
    /// </summary>
    public interface ILoadableResource {

        /// <summary>
        /// The resource path for this loadable resource (needs to be set in <see cref="Load(string, ref ILoadingParameters)"/>)
        /// </summary>
        public string ResourcePath { get; set; }


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
