namespace VengineX.Resources
{
    public interface ILoadingParameters {

        /// <summary>
        /// Used to automatically generate a resource path when loaded without specifing a resource path.
        /// </summary>
        /// <returns>The resource path</returns>
        public string ProvideResourcePath();
    }
}
