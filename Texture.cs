using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace HelloTriangle
{
    public class Texture
    {
        int Handle;

        public Texture(string path)
        {
           Handle = GL.GenTexture();
           Use();
           // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
           StbImage.stbi_set_flip_vertically_on_load(1); // This will correct that, making the texture display properly

           // Load the image.
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            Console.WriteLine("Textura activa en la unidad: " + unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}