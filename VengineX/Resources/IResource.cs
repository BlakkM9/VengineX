namespace VengineX.Resources
{
    /// <summary>
    /// Interface for classes that can be handled by the <see cref="ResourceManager"/>.<br/>
    /// Provides functionality for <see cref="ResourceManager.UnloadResource{T}(T)"/> (saves resource path in property of resource itself).
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// The resource path for this resource.<br/>
        /// Property is set in <see cref="ResourceManager.LoadResource{T}(string, ILoadingParameters)"/> or <see cref="ResourceManager.CacheResource{T}(string, T)"/>
        /// </summary>
        public string ResourcePath { get; set; }
    }
}
