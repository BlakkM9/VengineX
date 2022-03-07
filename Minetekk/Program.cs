using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.InteropServices;

namespace Minetekk
{
    public class Program
    {

        public static void Main(string[] args)
        {
            MinetekkGame game = new MinetekkGame();
            game.Start();
        }
    }
}