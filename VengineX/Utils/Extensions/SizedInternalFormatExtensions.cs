using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;

namespace VengineX.Utils.Extensions
{
    public static class SizedInternalFormatExtensions
    {
        public static int ChannelCount(this SizedInternalFormat internalFormat)
        {
            switch (internalFormat) {
                case SizedInternalFormat.Rgba8:
                case SizedInternalFormat.Rgba16f:
                    return 4;
                case SizedInternalFormat.R8:
                    return 1;
                default:
                    Logger.Log(
                        Severity.Error,
                        Tag.Loading,
                        "Channel count for pixel internal format " + internalFormat + " not know, assuming 4!");
                    return 4;
            }
        }
    }
}
