using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Core
{
    /// <summary>
    /// Interface for game screens (scene equivalent).<br/>
    /// Screens are managed by the static <see cref="ScreenManager"/>.
    /// </summary>
    public interface IScreen
    {
        /// <summary>
        /// Called when the screen is loaded.
        /// </summary>
        void Load();


        /// <summary>
        /// Called when the frame is updated.
        /// </summary>
        /// <param name="delta">Time passed since last render.</param>
        void Update(double delta);


        /// <summary>
        /// Called when the frame is rendered.
        /// </summary>
        /// <param name="delta">Time passed since last render.</param>
        void Render(double delta);


        /// <summary>
        /// Called when the window is resized.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        void Resize(int width, int height);


        /// <summary>
        /// Called when the screen is unloaded.
        /// </summary>
        void Unload();
    }
}
