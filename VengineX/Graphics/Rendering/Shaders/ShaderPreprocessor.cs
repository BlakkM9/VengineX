using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Debugging.Logging;


namespace VengineX.Graphics.Rendering.Shaders {

    /// <summary>
    /// Parses a shader file and processes preprocess directives that are not supported by GLSL.
    /// Supported preprocessor directives:
    /// #ifndef ...
    /// #define ...
    /// #endif
    /// #include ...
    /// </summary>
    public static class ShaderPreprocessor {

        private const string IFNDEF     = "#ifndef";
        private const string DEFINE     = "#define";
        private const string ENDIF      = "#endif";
        private const string INCLUDE    = "#include";


        private static readonly string[] DIRECTIVES = {
            IFNDEF,
            DEFINE,
            INCLUDE,
            ENDIF
        };


        /// <summary>
        /// Loads the shader and processes all preprocess directives
        /// </summary>
        /// <returns>Preprocessed shader source</returns>
        public static string ParseAndPreprocess(string shaderPath) {

            string source = ParseShader(shaderPath);
            return ProcessDirectives(shaderPath, source);
        }


        /// <summary>
        /// Loads shader source code from file
        /// </summary>
        /// <param name="shaderPath">Path to shader file</param>
        /// <returns>Shader source code</returns>
        private static string ParseShader(string shaderPath) {
            try {
                using StreamReader reader = new StreamReader(shaderPath, Encoding.UTF8);

                return reader.ReadToEnd();
            } catch (Exception e) {
                Logger.Log(Severity.Fatal, Tag.Shader, "Failed to read shader file " + shaderPath + ":\n" + e);
                return "";
            }
        }


        /// <summary>
        /// Processes directives
        /// </summary>
        /// <param name="rawSource">Unprocessed shader source</param>
        /// <returns>Processed shader code</returns>
        private static string ProcessDirectives(string shaderPath, string rawSource) {
            HashSet<string> defines = new HashSet<string>();
            bool inIf = false;

            string preprocessedSource = "";

            StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            string[] lines = rawSource.Split(Environment.NewLine, splitOptions);

            // Check every line
            foreach (string line in lines) {

                // Skip lines when in if (only check for endif to toggle)
                if (inIf) {
                    if (line.StartsWith(ENDIF)) {
                        inIf = false;
                    }

                    continue;
                }

                bool lineProcessed = false;

                // Check every supported directive
                for (int i = 0; i < DIRECTIVES.Length; i++) {

                    if (line.StartsWith(DIRECTIVES[i])) {
                        // Split by spaces and use first two (directive and argument)
                        string[] tokens = line.Split(" ", 2, splitOptions);
                        //Console.WriteLine(tokens[0] + " " + (tokens.Length > 1 ? tokens[1] : null));
                        
                        switch (tokens[0]) {
                            case IFNDEF:
                                // Check if defined, if so set inIf to true
                                if (defines.Contains(tokens[1])) { inIf = true; }
                                break;
                            case DEFINE:
                                // Add to defines
                                defines.Add(tokens[1]);
                                break;
                            case INCLUDE:
                                // Load and process include
                                // Strip " and ' from path
                                tokens[1] = tokens[1].Replace("\"", "").Replace("'", "");
                                // Path of file to include
                                string pathToInclude = Path.Combine(Path.GetDirectoryName(shaderPath), tokens[1]);
                                // Parse and preprocess include
                                string includeString = ParseAndPreprocess(pathToInclude);
                                preprocessedSource += includeString;
                                break;
                            case ENDIF:
                                // Dont copy endif if not in if (otherwise we won't come here anyways)
                                break;
                        }

                        lineProcessed = true;
                    }
                }

                if (!lineProcessed) {
                    // Copy line if no known directive found and processed
                    preprocessedSource += line + "\n";
                }
            }


            return preprocessedSource;
        }
    }
}
