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
        float[] vertices = 
        {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        };
        int vbo;
        int vao;
        Shader shader;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            //creating the shader program
            shader = new Shader("Resources/shader.vert", "Resources/shader.frag");
            // Vertex Buffer Object
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            //creating Vertex Array Object and bound it
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            //tell OpenGl how interpret vertex data
            GL.VertexAttribPointer(shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

        
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here
            shader.Use();
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


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