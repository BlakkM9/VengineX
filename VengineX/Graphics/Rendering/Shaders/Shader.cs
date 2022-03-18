using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;
using VengineX.Resources;

namespace VengineX.Graphics.Rendering.Shaders
{
    public class Shader : IBindable, IDisposable, ILoadableResource, IResource
    {
        public static int CurrentBoundShader { get; protected set; }

        /// <summary>
        /// OpenGL handle of this shader program.
        /// </summary>
        public int Handle { get; private set; }


        /// <summary>
        /// ResourcePath of this shader program (<see cref="ResourceManager"/> for more details on resource paths.
        /// </summary>
        public string ResourcePath { get; set; } = "";


        /// <summary>
        /// Loads the shader program (vert and frag) from disc, preprocesses, compiles and links vert and frag.<br/>
        /// <param name="loadingParameters">need to be <see cref="ShaderLoadingParameters"/></param>
        /// </summary>
        public void Load(ref ILoadingParameters loadingParameters)
        {
            ShaderLoadingParameters parameters = (ShaderLoadingParameters)loadingParameters;

            // Vertex Shader
            int vertexID = Compile(parameters.VertexPath, ShaderType.VertexShader);

            // Fragment Shader
            int fragmentID = Compile(parameters.FragmentPath, ShaderType.FragmentShader);

            // Link both
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexID);
            GL.AttachShader(Handle, fragmentID);

            GL.LinkProgram(Handle);
            // TODO check for errors when linking aswell

            // Cleanup
            GL.DetachShader(Handle, vertexID);
            GL.DetachShader(Handle, fragmentID);
            GL.DeleteShader(vertexID);
            GL.DeleteShader(fragmentID);
        }


        /// <summary>
        /// Compiles a shader and prints compile errors.
        /// Uses <see cref="ShaderPreprocessor"/> before compilation
        /// </summary>
        /// <returns>Shader ID</returns>
        private static int Compile(string shaderPath, ShaderType shaderType)
        {
            int shaderID = GL.CreateShader(shaderType);

            string shaderSource = ShaderPreprocessor.ParseAndPreprocess(shaderPath);
            GL.ShaderSource(shaderID, shaderSource);
            GL.CompileShader(shaderID);

            string infoLogVert = GL.GetShaderInfoLog(shaderID);
            if (infoLogVert != string.Empty)
            {
                Logger.Log(Severity.Error, Tag.Shader, "Failed to compile shader " + shaderPath + ": " + infoLogVert);
                Console.WriteLine(infoLogVert);
                GL.DeleteShader(shaderID);
            }

            return shaderID;
        }


        #region Uniform access

        public int GetUniformLocation(string uniformName)
        {
            int uniformLocation = GL.GetUniformLocation(Handle, uniformName);
            if (uniformLocation == -1)
            {
                Logger.Log(Severity.Error, Tag.Shader, $"Failed to find uniform {uniformName} in shader");
            }

            return uniformLocation;
        }


        public void SetUniformMat4(int uniformLocation, ref Matrix4 value)
        {
            Bind();
            GL.UniformMatrix4(uniformLocation, false, ref value);
        }


        /// <summary>
        /// Only use this function if uniforms are only set once.
        /// It performs a <see cref="GL.GetUniformLocation(int, string)"/> call!<br/>
        /// Cache the uniform location and use <see cref="SetUniformMat4(int, ref Matrix4)"/> if you're using it more than once!
        /// </summary>
        public void SetUniformMat4(string uniformName, ref Matrix4 value)
        {
            int uniformLocation = GetUniformLocation(uniformName);
            if (uniformLocation == -1)
            {
                Logger.Log(Severity.Error, Tag.Shader, $"Failed to find uniform {uniformName} in shader");
            }
            else
            {
                SetUniformMat4(uniformLocation, ref value);
            }
        }


        public void SetUniform1(int uniformLocation, bool value)
        {
            Bind();
            GL.Uniform1(uniformLocation, value ? 1 : 0);
        }


        public void SetUniform1(string uniformName, bool value)
        {
            int uniformLocation = GetUniformLocation(uniformName);
            if (uniformLocation == -1)
            {
                Logger.Log(Severity.Error, Tag.Shader, $"Failed to find uniform {uniformName} in shader");
            }
            else
            {
                SetUniform1(uniformLocation, value);
            }
        }


        public void SetUniform1(int uniformLocation, int value)
        {
            Bind();
            GL.Uniform1(uniformLocation, value);
        }


        /// <summary>
        /// Only use this function if uniforms are only set once.<br/>
        /// It performs a <see cref="GL.GetUniformLocation(int, string)"/> call!<br/>
        /// Cache the uniform location and use <see cref="SetUniform1(int, int)"/> if you're using it more than once!
        /// </summary>
        public void SetUniform1(string uniformName, int value)
        {

            int uniformLocation = GetUniformLocation(uniformName);
            if (uniformLocation == -1)
            {
                Logger.Log(Severity.Error, Tag.Shader, $"Failed to find uniform {uniformName} in shader");
            }
            else
            {
                SetUniform1(uniformLocation, value);
            }
        }


        public void SetUniform1(int uniformLocation, float value)
        {
            Bind();
            GL.Uniform1(uniformLocation, value);
        }


        public void SetUniformVec3(int uniformLoaction, ref Vector3 value)
        {
            Bind();
            GL.Uniform3(uniformLoaction, value);
        }


        public void SetUniformVec4(int uniformLoaction, ref Vector4 value)
        {
            Bind();
            GL.Uniform4(uniformLoaction, value);
        }


        /// <summary>
        /// Only use this function if uniforms are only set once.<br/>
        /// It performs a <see cref="GL.GetUniformLocation(int, string)"/> call!<br/>
        /// Cache the uniform location and use <see cref="SetUniformVec4(int, ref Vector4)"/> if you're using it more than once!
        /// </summary>
        public void SetUniformVec4(string uniformName, ref Vector4 value)
        {
            int uniformLocation = GetUniformLocation(uniformName);
            if (uniformLocation == -1)
            {
                Logger.Log(Severity.Error, Tag.Shader, $"Failed to find uniform {uniformName} in shader");
            }
            else
            {
                SetUniformVec4(uniformLocation, ref value);
            }
        }

        #endregion


        #region IBindable

        /// <summary>
        /// Binds this shader program to the current OpenGL renderer state.
        /// </summary>
        public void Bind()
        {
            // Only bind if this shader is not bound already
            if (CurrentBoundShader != Handle)
            {
                GL.UseProgram(Handle);
                CurrentBoundShader = Handle;
            }
        }


        /// <summary>
        /// Unbinds this shader program from the current OpenGL renderer state.<br/>
        /// Binds 0, usually not required.
        /// </summary>
        public void Unbind()
        {
            if (CurrentBoundShader != 0)
            {
                GL.UseProgram(0);
                CurrentBoundShader = 0;
            }
        }

        #endregion


        #region IDisposable

        private bool _disposedValue;


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteProgram(Handle);
                _disposedValue = true;
            }
        }


        ~Shader()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
