using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Graphics.Rendering.Shaders
{
    public class Uniform
    {
        /// <summary>
        /// Shader this uniform is located in.
        /// </summary>
        public Shader Shader { get; }

        /// <summary>
        /// The location of this uniform in its shader.
        /// </summary>
        public int Location { get; }

        public int Count { get; }

        /// <summary>
        /// The uniform type.
        /// </summary>
        public ActiveUniformType Type { get; }


        public Uniform(Shader shader, int location, int count, ActiveUniformType type)
        {
            Shader = shader;
            Location = location;
            Count = count;
            Type = type;
        }


        public void Set1(ref float value) => GL.ProgramUniform1(Shader.Handle, Location, value); 


        public void Set2(ref Vector2 value) => GL.ProgramUniform2(Shader.Handle, Location, ref value);


        public void Set3(ref Vector3 value) => GL.ProgramUniform3(Shader.Handle, Location, ref value);


        public void Set4(ref Vector4 value) => GL.ProgramUniform4(Shader.Handle, Location, ref value);


        public void SetMat4(ref Matrix4 value) => GL.ProgramUniformMatrix4(Shader.Handle, Location, false, ref value);
    }
}
