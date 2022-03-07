using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Config;
using VengineX.Core;
using VengineX.Debugging.Logging;

namespace Minetekk
{
    public class MinetekkGame : Game<GameSettings>
    {
        public override void Load()
        {
            Logger.Log(Severity.Error, Tag.Loading, "Load");
            Logger.LogRaw(Severity.Fatal, "Loading", "Load");
            Logger.Log(Severity.Warning, Tag.Loading, "Load");
            Logger.Log(Severity.Debug, Tag.Loading, "Load");
        }


        public override void Update(double delta)
        {
            // TEST
            KeyboardState kbs = Window.KeyboardState.GetSnapshot();

            if (kbs.IsKeyPressed(Keys.F)) {
                WindowMode[] windowModes = Enum.GetValues<WindowMode>();
                int index = Array.IndexOf(windowModes, Window.WindowMode);
                index++;

                if (index >= windowModes.Length)
                {
                    index = 0;
                }

                Window.WindowMode = windowModes[index];
            }
        }


        public override void Render(double delta)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Window.Context.SwapBuffers();
        }


        public override void Resize(int width, int height)
        {
            Logger.Log($"Resize {width}, {height}");
        }


        public override void Unload()
        {
            Logger.Log("Unoad");
        }
    }
}
