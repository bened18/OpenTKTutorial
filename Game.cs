using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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
        ){
            screenwidth = width;
            screenheight = height;
        }

        // -- Vertex Input
        float[] vertices = 
        {
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
        uint[] indices = 
        {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        Vector3 position;
        Vector3 front;
        Vector3 up;
        int screenwidth;
        int screenheight;
        double time;
        int vbo;
        int vao;
        int ebo;
        Shader shader;
        Texture texture;
        Texture texture2;
        Matrix4 model;
        Matrix4 view;
        Matrix4 projection;
        Vector2 lastPos;
        double yaw;
        double pitch;
        bool firstMove = true;
        


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

            // Transformation matrix
            //Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
            //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
            //Matrix4 trans = rotation * scale;
            //sent the matrix to the vertex shader
            //shader.SetMatrix4("transform", trans);

            // -- Transformation
            // Note that we're translating the scene in the reverse direction of where we want to move.
            //view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

            position = new Vector3(0.0f, 0.0f,  3.0f);
            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = new Vector3(0.0f, 1.0f,  0.0f);
            

            // Static Cursor
            CursorState = CursorState.Grabbed;
            
            // Z-buffer
            GL.Enable(EnableCap.DepthTest);
            
            

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            time += 32.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Bind the vao
            GL.BindVertexArray(vao);

            //Code goes here
            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);
            shader.Use();

            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));
            //projection matrix to perspective
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), screenwidth / screenheight, 0.1f, 100.0f);
            // Setting up the camera
            // Look at
            view = Matrix4.LookAt(position, position + front, up);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);
            
            // update color via uniform
            //int vertexColorLocation = shader.GetUniformLocation("rectangleColor");
            //GL.Uniform4(vertexColorLocation, 0.2f, 0.0f, 1.0f, 1.0f);

            //Draw the triangles
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            //Draw Rectangles
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            
            //Draw Cube
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

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

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            float speed = 1.5f;
            float sensitivity = 0.3f;

            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed * (float)e.Time; //Down
            } 


            // Get the mouse state
            var mouse = MouseState;

            if(firstMove) // this bool variable is initially set to true
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else if (IsFocused)
            {
                float deltaX = mouse.X - lastPos.X;
                float deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                yaw += deltaX * sensitivity;
                pitch -= deltaY * sensitivity;

                if(pitch > 89.0f)
                {
                    pitch = 89.0f;
                }
                else if(pitch < -89.0f)
                {
                    pitch = -89.0f;
                }
                else
                {
                    pitch -= deltaX * sensitivity;
                }
            }
            
        
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);


        } 

    }
}