using System.Runtime.InteropServices;
using System.Text;
using VengineX.Debugging.Logging;

namespace VengineX.Wrappers.Stbi
{
    /// <summary>
    /// Enum for different available loading functions of images.<br/>
    /// Used for texture loading parameters.
    /// </summary>
    public enum LoadingFunction
    {
        Load,
        Load16,
        LoadF,
    }


    /// <summary>
    /// Wrapper for stb_image
    /// </summary>
    public static class StbiWrapper
    {
        private const string dllFilePath = "lib/stbilib.dll";


        #region stbilib.dll imports

        // Loading images
        // RGB(A)8
        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static byte* load(ref byte filename, ref int x, ref int y, ref int channels_in_file, int desired_channels);


        // RGB(A)16f ??
        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static ushort* load_16(ref byte filename, ref int x, ref int y, ref int channels_in_file, int desired_channels);


        // RGB(A)32f ??
        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static float* loadf(ref byte filename, ref int x, ref int y, ref int channels_in_file, int desired_channels);


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private unsafe extern static void image_free(void* retval_from_stbi_load);


        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void set_flip_vertically_on_load(int flag_true_if_should_flip);

        #endregion


        #region C# Wrapping functions

        /// <summary>
        /// Wrapper function for stbi_load.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <param name="desiredChannels">Amount of desired channels.</param>
        public unsafe static Image Load(string path, int desiredChannels)
        {
            int width = 0;
            int height = 0;
            int channels = 0;

            byte[] pathBytes = Encoding.ASCII.GetBytes(path);

            byte* imageData = load(ref pathBytes[0], ref width, ref height, ref channels, desiredChannels);

            if (imageData == null)
            {
                Logger.Log(Severity.Error, Tag.Loading, "Failed to load texture from " + path);
            }

            return new Image(imageData, width, height, desiredChannels, 1);
        }


        /// <summary>
        /// Wrapper function for stbi_load_16.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <param name="desiredChannels">Amount of desired channels.</param>
        public unsafe static Image Load16(string path, int desiredChannels)
        {
            int width = 0;
            int height = 0;
            int channels = 0;

            byte[] pathBytes = Encoding.ASCII.GetBytes(path);

            byte* imageData = (byte*)load_16(ref pathBytes[0], ref width, ref height, ref channels, desiredChannels);

            if (imageData == null)
            {
                Logger.Log(Severity.Error, Tag.Loading, "Failed to load texture from " + path);
            }

            return new Image(imageData, width, height, desiredChannels, 2);
        }


        /// <summary>
        /// Wrapper function for stbi_loadf.<br/>
        /// Loads image as (linear) float to preserve full dynamic range.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <param name="desiredChannels">Amount of desired channels.</param>
        public unsafe static Image LoadF(string path, int desiredChannels)
        {
            int width = 0;
            int height = 0;
            int channels = 0;

            byte[] pathBytes = Encoding.ASCII.GetBytes(path);

            byte* imageData = (byte*)loadf(ref pathBytes[0], ref width, ref height, ref channels, desiredChannels);

            if (imageData == null)
            {
                Logger.Log(Severity.Error, Tag.Loading, "Failed to load texture from " + path);
            }

            return new Image(imageData, width, height, desiredChannels, 4); ;
        }


        /// <summary>
        /// Wrapper function for 
        /// </summary>
        /// <param name="imageData"></param>
        public unsafe static void ImageFree(byte* imageData) => image_free(imageData);


        /// <summary>
        /// Flips images vertically on load. State is kept.
        /// </summary>
        public static void SetFlipVerticallyOnLoad(bool flip) => set_flip_vertically_on_load(flip ? 1 : 0);

        #endregion
    }
}
