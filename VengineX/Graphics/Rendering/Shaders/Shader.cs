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
        /// <summary>
        /// The ID of the shader that is currently bound to the renderer state.
        /// </summary>
        public static int CurrentBoundShader { get; protected set; }


        /// <summary>
        /// ResourcePath of this shader program (<see cref="ResourceManager"/> for more details on resource paths.
        /// </summary>
        public string ResourcePath { get; set; } = "";


        /// <summary>
        /// OpenGL handle of this shader program.
        /// </summary>
        public int Handle { get; private set; }

        /// <summary>
        /// Maps string uniform names to uniform locations.
        /// </summary>
        private Dictionary<string, int> _uniformLocations;

        /// <summary>
        /// Maps uniform locations to uniforms.
        /// </summary>
        private Dictionary<int, Uniform> _uniforms;

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


            // Get all uniforms in this shader
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            _uniformLocations = new Dictionary<string, int>();
            _uniforms = new Dictionary<int, Uniform>();

            for (int i = 0; i < uniformCount; i++)
            {
                string uniformName = GL.GetActiveUniform(Handle, i, out int size, out ActiveUniformType type);
                int uniformLocation = GL.GetUniformLocation(Handle, uniformName);
                _uniformLocations.Add(uniformName, uniformLocation);
                _uniforms.Add(uniformLocation, new Uniform(this, uniformLocation, size, type));
            }
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
            return _uniformLocations[uniformName];
        }


        public Uniform GetUniform(string uniformName)
        {
            return _uniforms[GetUniformLocation(uniformName)];
        }

        public Uniform GetUniform(int uniformIndex)
        {
            return _uniforms[uniformIndex];
        }

        public void SetUniform1(string uniformName, ref float value) => GetUniform(uniformName).Set1(ref value);

        public void SetUniform1(int uniformLocation, ref float value) => GetUniform(uniformLocation).Set1(ref value);

        public void SetUniform2(string uniformName, ref Vector2 value) => GetUniform(uniformName).Set2(ref value);

        public void SetUniform2(int uniformLocation, ref Vector2 value) => GetUniform(uniformLocation).Set2(ref value);

        public void SetUniform3(string uniformName, ref Vector3 value) => GetUniform(uniformName).Set3(ref value);

        public void SetUniform3(int uniformLocation, ref Vector3 value) => GetUniform(uniformLocation).Set3(ref value);

        public void SetUniform4(string uniformName, ref Vector4 value) => GetUniform(uniformName).Set4(ref value);

        public void SetUniform4(int uniformLocation, ref Vector4 value) => GetUniform(uniformLocation).Set4(ref value);

        public void SetUniformMat4(string uniformName, ref Matrix4 value) => GetUniform(uniformName).SetMat4(ref value);

        public void SetUniformMat4(int uniformLocation, ref Matrix4 value) => GetUniform(uniformLocation).SetMat4(ref value);


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

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteProgram(Handle);
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        ~Shader()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        /// <summary>
        /// Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
