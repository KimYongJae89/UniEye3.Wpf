using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public class TextureImage
    {
        private Size textureSize;
        private uint textureHandle = 0;

        private bool Create(PixelInternalFormat internalFormat, Size size, PixelFormat format, PixelType type, IntPtr imagePtr)
        {
            Delete();

            GL.GenTextures(1, out textureHandle);

            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, size.Width, size.Height, 0, format, type, imagePtr);

            textureSize = size;

            return true;
        }

        public void Delete()
        {
            if (textureHandle != 0)
            {
                GL.DeleteTextures(1, ref textureHandle);
                textureHandle = 0;
            }
        }

        public bool IsValid()
        {
            return (textureHandle != 0);
        }

        public bool Bind()
        {
            if (!IsValid())
            {
                return false;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);

            return true;
        }
    }
}
