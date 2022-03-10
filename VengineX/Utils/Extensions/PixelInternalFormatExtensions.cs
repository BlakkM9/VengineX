using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;

namespace VengineX.Utils.Extensions
{
    public static class PixelInternalFormatExtensions
    {
        public static int ChannelCount(this PixelInternalFormat pixelInternalFormat)
        {
            switch (pixelInternalFormat) {
                case PixelInternalFormat.Rgba8:
                    return 4;
                case PixelInternalFormat.Rgb16f:
                case PixelInternalFormat.Rgb8:
                    return 3;
                case PixelInternalFormat.R8:
                    return 1;
                default:
                    Logger.Log(
                        Severity.Error,
                        Tag.Loading,
                        "Channel count for pixel internal format " + pixelInternalFormat + " not know, assuming 4!");
                    return 4;
            }
        }
    }
}
