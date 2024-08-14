using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloTriangle
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string title) :
        base
        (
            GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = (width, height), Title = title
            }
        ){}

        // -- Vertex Input
        private readonly float[] vertices =
        {
            // positions        // colors
            0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
            -0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom left
            0.0f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f    // top 
        };
        uint[] indices = 
        {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        int vbo;
        int vao;
        int ebo;
        Shader shader;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            //creating the shader program
            shader = new Shader("Resources/shader.vert", "Resources/shader.frag");
            // Vertex Buffer Object VBO
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            //creating Vertex Array Object and bound it VAO
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            //tell OpenGl how interpret vertex data
            // position vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // color vertices
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            // Element Buffer Object EBO
            //ebo = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here
            shader.Use();
            // update color via uniform
            //int vertexColorLocation = shader.GetUniformLocation("rectangleColor");
            //GL.Uniform4(vertexColorLocation, 0.2f, 0.0f, 1.0f, 1.0f);

            //Bind the vao
            GL.BindVertexArray(vao);

            //Draw the triangles
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Delete and Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vbo);

            //Delete the shader program
            shader.Dispose();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }


    }
}