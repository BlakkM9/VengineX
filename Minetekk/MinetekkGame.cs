using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
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

            Window.CursorGrabbed = true;
            Window.CursorVisible = true;
        }


        public override void Update(double delta)
        {

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
