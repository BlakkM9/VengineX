using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Resources.Stbi {

    // TODO implement error handling (somehow)
    public static class StbiWrapper {

        public enum LoadingFunction
        {
            Load,
            Load16,
            LoadF,
        }


        private const string dllFilePath = "_lib/stbilib.dll";


        #region stbilib.dll imports

        // Temporary test function
        [DllImport(dllFilePath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe extern static byte* test(ref byte str);


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
        public unsafe static StbiImage Load(string path, int desiredChannels) {
            int width = 0;
            int height = 0;
            int channels = 0;

            byte[] pathBytes = Encoding.ASCII.GetBytes(path);

            byte* imageData = load(ref pathBytes[0], ref width, ref height, ref channels, desiredChannels);
            return new StbiImage(imageData, width, height, desiredChannels, 1);
        }


        /// <summary>
        /// Wrapper function for stbi_load_16.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <param name="desiredChannels">Amount of desired channels.</param>
        public unsafe static StbiImage Load16(string path, int desiredChannels)
        {
            int width = 0;
            int height = 0;
            int channels = 0;

            byte[] pathBytes = Encoding.ASCII.GetBytes(path);

            byte* imageData = (byte*)load_16(ref pathBytes[0], ref width, ref height, ref channels, desiredChannels);
            return new StbiImage(imageData, width, height, desiredChannels, 2);
        }


        /// <summary>
        /// Wrapper function for stbi_loadf.<br/>
        /// Loads image as (linear) float to preserve full dynamic range.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <param name="desiredChannels">Amount of desired channels.</param>
        public unsafe static StbiImage LoadF(string path, int desiredChannels)
        {
            int width = 0;
            int height = 0;
            int channels = 0;

            byte[] pathBytes = Encoding.ASCII.GetBytes(path);

            byte* imageData = (byte*)loadf(ref pathBytes[0], ref width, ref height, ref channels, desiredChannels);
            return new StbiImage(imageData, width, height, desiredChannels, 4); ;
        }


        /// <summary>
        /// Wrapper function for 
        /// </summary>
        /// <param name="imageData"></param>
        public unsafe static void ImageFree(byte* imageData)
        {
            image_free(imageData);
        }


        public static void SetFlipVerticallyOnLoad(bool flip)
        {
            set_flip_vertically_on_load(flip ? 1 : 0);
        }

        #endregion
    }
}
