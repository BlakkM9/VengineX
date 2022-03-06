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

namespace Minetekk
{
    public class MinetekkGame : Game<GameSettings>
    {
        public override void Load()
        {
            Console.WriteLine("Load");
        }


        public override void Update(double delta)
        {
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
            Console.WriteLine($"Resize {width}, {height}");
        }


        public override void Unload()
        {
            Console.WriteLine("Unoad");
        }
    }
}
