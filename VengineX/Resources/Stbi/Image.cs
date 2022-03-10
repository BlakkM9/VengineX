using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Resources.Stbi
{
    /// <summary>
    /// Class for images loaded with <see cref="StbiWrapper"/>.
    /// </summary>
    public class Image : IDisposable
    {
        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the image in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Number of channels in this image.
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// How much bytes of data one pixel has.
        /// </summary>
        public int BytesPerPixel { get; }

        /// <summary>
        /// Length of the data array.
        /// </summary>
        public int DataLength { get; }

        /// <summary>
        /// Wrapped data.
        /// </summary>
        public IntPtr Data
        {
            get
            {
                unsafe
                {
                    return (IntPtr)_data;
                }
            }
        }

        /// <summary>
        /// Original data pointer.
        /// </summary>
        private unsafe readonly byte* _data;


        public unsafe Image(byte* data, int width, int height, int channels, int bytesPerPixel)
        {
            _data = data;
            Width = width;
            Height = height;
            Channels = channels;
            BytesPerPixel = bytesPerPixel;
            DataLength = width * height * channels;
        }


        #region IDisposable

        protected bool _disposedValue = false;

        ~Image()
        {
            Dispose();
        }


        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                unsafe
                {
                    StbiWrapper.ImageFree(_data);
                }
                _disposedValue = true;
            }
        }


        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
