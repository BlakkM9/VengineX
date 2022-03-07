using System;

namespace VengineX.Resources {

    /// <summary>
    /// Interface that represents a resource that can be loaded from files.
    /// Implement this if you want to load something with the <see cref="ResourceManager"/> (aswell as <see cref="IDisposable"/>)
    /// </summary>
    public interface ILoadableResource {
        void Load(string resourcePath, string fileType, ref ILoadingParameters loadingParameters);
    }
}
