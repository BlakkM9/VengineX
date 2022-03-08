using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;

namespace VengineX.Config
{
    /// <summary>
    /// Derive from this class and put your own in the <see cref="Game{T}"/>'s type parameter if you need more settings.<br/>.
    /// </summary>
    public class GameSettings : GameSettingsBase
    {
        /// <summary>
        /// <see cref="GameWindowSettings.RenderFrequency"/>
        /// </summary>
        public double RenderFrequency
        {
            get => GetDouble("Window.RenderFrequency");
            set => Set("Window.RenderFrequency", value);
        }


        /// <summary>
        /// <see cref="GameWindowSettings.UpdateFrequency"/>
        /// </summary>
        public double UpdateFrequency
        {
            get => GetDouble("Window.UpdateFrequency");
            set => Set("Window.UpdateFrequency", value);
        }


        /// <summary>
        /// Width of the game window.
        /// </summary>
        public int ResolutionX
        {
            get => GetInt("Window.ResolutionX");
            set => Set("Window.ResolutionX", value);
        }


        /// <summary>
        /// Height of the game window.
        /// </summary>
        public int ResolutionY
        {
            get => GetInt("Window.ResolutionY");
            set => Set("Window.ResolutionY", value);
        }


        /// <summary>
        /// The monitor the game's window should be displayed on.<br/>
        /// Index of <see cref="Monitors.GetMonitors()"/> (first is always primary).
        /// </summary>
        public int TargetMonitor
        {
            get => GetInt("Window.TargetMonitor");
            set => Set("Window.TargetMonitor", value);
        }


        /// <summary>
        /// The monitor the game's window should be displayed on.<br/>
        /// Index of <see cref="Monitors.GetMonitors()"/> (first is always primary).
        /// </summary>
        public WindowMode WindowMode
        {
            get => Enum.Parse<WindowMode>(GetString("Window.WindowMode"));
            set => Set("Window.WindowMode", value.ToString());
        }


        /// <summary>
        /// The monitor the game's window should be displayed on.<br/>
        /// Index of <see cref="Monitors.GetMonitors()"/> (first is always primary).
        /// </summary>
        public bool DebugMode
        {
            get => GetBool("Debug.Enabled");
            set => Set("Debug.Enabled", value);
        }


        /// <summary>
        /// Fills <see cref="GameSettingsBase._sections"/> with default settings.<br/>
        /// Override this function if you've overwritten <see cref="GameSettingsBase"/><br/>
        /// and make sure, you're creating all your default settings here.
        /// </summary>
        protected override void CreateDefaultSettings()
        {
            // 0 means unlimited (hardware limited) frequency.
            RenderFrequency = 0;
            UpdateFrequency = 0;
            // Primary monitor is always the first one.
            MonitorInfo monitor = Monitors.GetMonitors()[0];
            ResolutionX = monitor.HorizontalResolution;
            ResolutionY = monitor.VerticalResolution;
            TargetMonitor = 0;
            WindowMode = WindowMode.Borderless;


            DebugMode = true;
        }
    }
}
