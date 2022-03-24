using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace VengineX.Graphics.Rendering.Shaders
{
    /// <summary>
    /// Class that represents a uniform of a shader.
    /// </summary>
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

        /// <summary>
        /// Size of this uniform.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// The uniform type.
        /// </summary>
        public ActiveUniformType Type { get; }

        /// <summary>
        /// Creates a new uniform..
        /// </summary>
        /// <param name="shader">Shader this uniform is located in.</param>
        /// <param name="location">The uniform location.</param>
        /// <param name="size">The size of this uniform.</param>
        /// <param name="type">The type of this uniform.</param>
        public Uniform(Shader shader, int location, int size, ActiveUniformType type)
        {
            Shader = shader;
            Location = location;
            Size = size;
            Type = type;
        }


        public void Set1(float value) => GL.ProgramUniform1(Shader.Handle, Location, value);

        public void Set1(float[] values) => GL.ProgramUniform1(Shader.Handle, Location, values.Length, values);

        public void Set1(int value) => GL.ProgramUniform1(Shader.Handle, Location, value);

        public void Set1(int[] values) => GL.ProgramUniform1(Shader.Handle, Location, values.Length, values);

        public void Set2(ref Vector2 value) => GL.ProgramUniform2(Shader.Handle, Location, ref value);

        public void Set3(ref Vector3 value) => GL.ProgramUniform3(Shader.Handle, Location, ref value);

        public void Set4(ref Vector4 value) => GL.ProgramUniform4(Shader.Handle, Location, ref value);

        public void SetMat4(ref Matrix4 value) => GL.ProgramUniformMatrix4(Shader.Handle, Location, false, ref value);
    }
}
