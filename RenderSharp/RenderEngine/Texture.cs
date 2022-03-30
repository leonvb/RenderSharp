using System.Collections.Generic;
using OpenGL;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RenderSharp
{
    public class Texture
    {
        public uint Handle;

        public Texture(string ImageFile)
        {
            this.Handle = GL.glGenTexture();

            Use();

            // Load Image
            Image<Rgba32> image = Image.Load<Rgba32>(ImageFile);
            List<byte> pixels = GetImagePixels(image);

            GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, GL.GL_REPEAT);
            GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, GL.GL_REPEAT);

            GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_NEAREST);
            GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_NEAREST);

            unsafe
            {
                fixed (byte* pixelsPointer = &pixels.ToArray()[0])
                {
                    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_RGBA, image.Width, image.Height, 0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, pixelsPointer);
                }
            }

            GL.glGenerateMipmap(GL.GL_TEXTURE_2D);
        }

        public List<byte> GetImagePixels(Image<Rgba32> Image)
        {
            List<byte> pixels = new List<byte>(4 * Image.Width * Image.Height);

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            Image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.

            Image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var row = accessor.GetRowSpan(y);

                    for (int x = 0; x < row.Length; x++)
                    {
                        pixels.Add(row[x].R);
                        pixels.Add(row[x].G);
                        pixels.Add(row[x].B);
                        pixels.Add(row[x].A);
                    }
                }
            });

            return pixels;
        }

        public void Use(int unit = GL.GL_TEXTURE0)
        {
            GL.glActiveTexture(unit);
            GL.glBindTexture(GL.GL_TEXTURE_2D, this.Handle);
        }
    }
}
