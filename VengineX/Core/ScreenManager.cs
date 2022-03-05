using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Core
{
    public static class ScreenManager
    {

        /// <summary>
        /// Screen that is currently active
        /// </summary>
        public static IScreen? CurrentScreen { get; set; }
    }
}
