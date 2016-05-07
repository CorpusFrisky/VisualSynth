using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

//Whole bunch of this taken from the following OpenTK tutorial:
// http://www.opentk.com/node/3693

//TODO: Log errors instead of writing to console
namespace CorpusFrisky.VisualSynth.SynthModules.ViewModels.Effects.Shaders
{
    public class Shader : IDisposable
    {

        /// <summary>
        /// Type of Shader
        /// </summary>
        public enum Type
        {
            Vertex = 0x1,
            Fragment = 0x2
        }

        private int _program = 0;
        private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();

        public Shader(String vertexShaderSource = null, String fragmentShaderSource = null)
        {
            if (!IsSupported)
            {
                Console.WriteLine("Failed to create Shader." +
                    Environment.NewLine + "Your system doesn't support Shader.", "Error");
                return;
            }

            Compile(vertexShaderSource, fragmentShaderSource);
        }

        public void Dispose()
        {
            if (_program != 0)
                GL.DeleteProgram(_program);
        }

        public static bool IsSupported
            => (new Version(GL.GetString(StringName.Version).Substring(0, 3)) >= new Version(2, 0) ? true : false);

        public void SetVariable(string name, float x, float? y = null, float? z = null, float? w = null)
        {
            Action<int> action = null;
            if (y == null)
            {
                action = location => GL.Uniform1(location, x);
            }
            else if (z == null)
            {
                action = location => GL.Uniform2(location, x, y.Value);
            }
            else if (w == null)
            {
                action = location => GL.Uniform3(location, x, y.Value, z.Value);
            }
            else
            {
                action = location => GL.Uniform4(location, x, y.Value, z.Value, w.Value);
            }

            SetVariable(name, action);
        }

        public void SetVariable(string name, Matrix4 matrix)
        {
            SetVariable(name, location => GL.UniformMatrix4(location, false, ref matrix));
        }

        public void SetVariable(string name, Vector2 vector)
        {
            SetVariable(name, vector.X, vector.Y);
        }

        public void SetVariable(string name, Vector3 vector)
        {
            SetVariable(name, vector.X, vector.Y, vector.Z);
        }

        public void SetVariable(string name, Color color)
        {
            SetVariable(name, color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public static void Bind(Shader shader)
        {
            if (shader != null && shader._program > 0)
            {
                GL.UseProgram(shader._program);
            }
            else
            {
                GL.UseProgram(0);
            }
        }

        #region Helper Methods

        private bool Compile(String vertexSource = null, String fragmentSource = null)
        {
            var statusCode = -1;
            var info = "";

            if (String.IsNullOrEmpty(vertexSource) &&
                String.IsNullOrEmpty(fragmentSource))
            {
                Console.WriteLine("Failed to compile Shader." +
                    Environment.NewLine + "Nothing to Compile.", "Error");
                return false;
            }

            if (_program > 0)
                GL.DeleteProgram(_program);

            _variables.Clear();

            _program = GL.CreateProgram();

            if (vertexSource != "")
            {
                int vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vertexSource);
                GL.CompileShader(vertexShader);
                GL.GetShaderInfoLog(vertexShader, out info);
                GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out statusCode);

                if (statusCode != 1)
                {
                    Console.WriteLine("Failed to Compile Vertex Shader Source." +
                        Environment.NewLine + info + Environment.NewLine + "Status Code: " + statusCode.ToString());

                    GL.DeleteShader(vertexShader);
                    GL.DeleteProgram(_program);
                    _program = 0;

                    return false;
                }

                GL.AttachShader(_program, vertexShader);
                GL.DeleteShader(vertexShader);
            }

            if (fragmentSource != "")
            {
                int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fragmentSource);
                GL.CompileShader(fragmentShader);
                GL.GetShaderInfoLog(fragmentShader, out info);
                GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out statusCode);

                if (statusCode != 1)
                {
                    Console.WriteLine("Failed to Compile Fragment Shader Source." +
                        Environment.NewLine + info + Environment.NewLine + "Status Code: " + statusCode.ToString());

                    GL.DeleteShader(fragmentShader);
                    GL.DeleteProgram(_program);
                    _program = 0;

                    return false;
                }

                GL.AttachShader(_program, fragmentShader);
                GL.DeleteShader(fragmentShader);
            }

            GL.LinkProgram(_program);
            GL.GetProgramInfoLog(_program, out info);
            GL.GetProgram(_program, ProgramParameter.LinkStatus, out statusCode);

            if (statusCode != 1)
            {
                Console.WriteLine("Failed to Link Shader rogram." +
                    Environment.NewLine + info + Environment.NewLine + "Status Code: " + statusCode.ToString());

                GL.DeleteProgram(_program);
                _program = 0;

                return false;
            }

            return true;
        }

        private int GetVariableLocation(string name)
        {
            if (_variables.ContainsKey(name))
                return _variables[name];

            int location = GL.GetUniformLocation(_program, name);

            if (location != -1)
                _variables.Add(name, location);
            else
                Console.WriteLine("Failed to retrieve Variable Location." +
                    Environment.NewLine + "Variable Name not found.", "Error");

            return location;
        }

        private void SetVariable(string name, Action<int> setterAction)
        {
            if (_program > 0)
            {
                GL.UseProgram(_program);

                int location = GetVariableLocation(name);
                if (location != -1)
                {
                    setterAction(location);
                }

                GL.UseProgram(0);
            }
        }

        #endregion
    }
}