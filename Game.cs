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
            //Position          Texture coordinates
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
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
        Texture texture;
        Texture texture2;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Vertex Buffer Object VBO
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            
            // creating Vertex Array Object and bound it VAO
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            // Element Buffer Object EBO
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // creating the shader program
            shader = new Shader("Resources/shader.vert", "Resources/shader.frag");
            shader.Use();

            //tell OpenGl how interpret vertex data
            // position vertices
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
            // texture position vertices for a rectangle
            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            // creating the texture
            texture = new Texture("Textures/container.jpg");
            texture.Use(TextureUnit.Texture0);
            texture2 = new Texture("Textures/awesomeface.png");
            texture2.Use(TextureUnit.Texture1);
            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);
            

            

            // color vertices for a triangle
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);
            
            

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Bind the vao
            GL.BindVertexArray(vao);

            //Code goes here
            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);
            shader.Use();
            
            // update color via uniform
            //int vertexColorLocation = shader.GetUniformLocation("rectangleColor");
            //GL.Uniform4(vertexColorLocation, 0.2f, 0.0f, 1.0f, 1.0f);

            

            //Draw the triangles
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            //Draw Rectangles
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


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