using System;
using System.IO;
using OpenGL;
using GlmSharp;

namespace RenderSharp
{
    public class Shader
    {
        bool _disposed = false;

        uint VertexShaderID;
        uint FragmentShaderID;

        public uint ProgramID;

        public Shader()
        {
            string vertexShaderSource = ReadShaderFile(@"Resources\Shaders\Texture_VertexShader.glsl");
            string fragmentShaderSource = ReadShaderFile(@"Resources\Shaders\Texture_FragmentShader.glsl");

            // Generate and Bind Shaders
            VertexShaderID = CompileShader(GL.GL_VERTEX_SHADER, vertexShaderSource);
            FragmentShaderID = CompileShader(GL.GL_FRAGMENT_SHADER, fragmentShaderSource);

            // Linking
            ProgramID = GL.glCreateProgram();
            GL.glAttachShader(ProgramID, VertexShaderID);
            GL.glAttachShader(ProgramID, FragmentShaderID);

            GL.glLinkProgram(ProgramID);

            // Cleanup Shaders after linking
            GL.glDetachShader(ProgramID, VertexShaderID);
            GL.glDetachShader(ProgramID, FragmentShaderID);
            GL.glDeleteShader(VertexShaderID);
            GL.glDeleteShader(FragmentShaderID);
        }

        public uint CompileShader(int shader_type, string shader_source)
        {
            uint shaderID = GL.glCreateShader(shader_type);
            GL.glShaderSource(shaderID, shader_source);
            GL.glCompileShader(shaderID);

            string compileInfo = GL.glGetShaderInfoLog(shaderID);
            if (compileInfo != "")
                Console.WriteLine(compileInfo);
            else
            {
                if (shader_type == GL.GL_VERTEX_SHADER)
                    Console.WriteLine("Vertex shader compile success");
                else if(shader_type == GL.GL_FRAGMENT_SHADER)
                    Console.WriteLine("Fragment shader compile success");
            }

            return shaderID;
        }

        private string ReadShaderFile(string FileName)
        {
            try
            {
                return File.ReadAllText(FileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to read shader source file");
            }
        }

        public void Use()
        {
            GL.glUseProgram(ProgramID);
        }

        public void SetMatrix4(string uniformName, mat4 matrix)
        {
            int location = GL.glGetUniformLocation(ProgramID, uniformName);

            unsafe
            {
                fixed (float* matrixPointer = &matrix.Values1D[0])
                {
                    GL.glUniformMatrix4fv(location, 1, false, matrixPointer);
                }
            }
        }

        public void SetInt(string textureUniformName, int value)
        {
            int location = GL.glGetUniformLocation(ProgramID, textureUniformName);
            GL.glUniform1i(location, value);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.glDeleteProgram(ProgramID);

                _disposed = true;
            }
        }

        ~Shader()
        {
            GL.glDeleteProgram(ProgramID);
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
    }
}
