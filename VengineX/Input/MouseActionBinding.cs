using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;

namespace VengineX.Input
{
    public class MouseActionBinding : Binding<bool>
    {
        public MouseButton Button { get; set; }

        
        public MouseActionBinding(MouseButton button)
        {
            Button = button;
        }


        public override void UpdateValue(InputManager input)
        {
            MouseState ms = input.MouseState;

            Value = ms.IsButtonDown(Button);
        }
    }
}
