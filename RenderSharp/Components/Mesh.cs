using GlmSharp;
using OpenGL;
using RenderSharp.EntityComponentSystem;

namespace RenderSharp
{
    public class Mesh : Component
    {
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public float[] UVs { get; set; }

        public uint VBO; // Vertex Buffer Object
        public uint VAO; // Vertex Array Object
        public uint EBO; // Element Buffer Object

        public Transform Transform;

        public static Mesh Cube()
        {
            Mesh mesh = new Mesh();

            //mesh.Transform = new Transform(new vec3(0.0f, 0.0f, 0.0f), new vec3(0.0f, 0.0f, 0.0f), new vec3(1.0f, 1.0f, 1.0f));

            mesh.Vertices = new float[] {
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
            };
            mesh.Indices = new uint[]
            {
                0, 1, 2, // Triangle 1
                3, 4, 5,  // Triangle 2

                6, 7, 8,  // Triangle 3
                9, 10, 11,  // Triangle 4

                12, 13, 14,  // Triangle 5
                15, 16, 17,  // Triangle 6

                18, 19, 20,  // Triangle 7
                21, 22, 23,  // Triangle 8

                24, 25, 26,  // Triangle 9
                27, 28, 29,  // Triangle 10

                30, 31, 32,  // Triangle 11
                33, 34, 35,  // Triangle 12

            };
            mesh.UVs = new float[0];

            mesh.Init_VertexBufferObject();
            mesh.Init_VertexArrayObject();
            mesh.Init_ElementBufferObject();

            return mesh;
        }

        public void Init_VertexBufferObject()
        {
            VBO = GL.glGenBuffer();
            GL.glBindBuffer((int)GL.GL_ARRAY_BUFFER, VBO);

            unsafe
            {
                fixed (float* vertexPointer = &this.Vertices[0])
                {
                    GL.glBufferData(GL.GL_ARRAY_BUFFER, this.Vertices.Length * sizeof(float), vertexPointer, GL.GL_STATIC_DRAW);
                }
            }
        }

        public void Init_VertexArrayObject()
        {
            VAO = GL.glGenVertexArray();
            GL.glBindVertexArray(VAO);

            // AttribPointers
            unsafe
            {
                // Vertex Positions
                GL.glVertexAttribPointer(0, 3, GL.GL_FLOAT, false, 5 * sizeof(float), (void*)0);
                GL.glEnableVertexAttribArray(0);

                // UVs
                GL.glVertexAttribPointer(1, 2, GL.GL_FLOAT, false, 5 * sizeof(float), (void*)(3 * sizeof(float)));
                GL.glEnableVertexAttribArray(1);
            }
        }

        public void Init_ElementBufferObject()
        {
            EBO = GL.glGenBuffer();
            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, EBO);

            unsafe
            {
                fixed (uint* indexPointer = &this.Indices[0])
                {
                    GL.glBufferData(GL.GL_ELEMENT_ARRAY_BUFFER, this.Indices.Length * sizeof(uint), indexPointer, GL.GL_STATIC_DRAW);
                }
            }
        }

        public override void Start()
        {
            this.Transform = this.Entity.WorldTransform;
        }
    }
}
