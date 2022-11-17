using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public class LightMap
    {
        public TextureColor TextureColor { get; set; } = null;

        private TextureImage textureImage = null;

        public void Set(int nMap, SurfaceMode surfaceMode)
        {
            //            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.5f);

            switch (nMap)
            {
                case 0:
                    SetMap0(surfaceMode);
                    break;
                case 1:
                    SetMap1(surfaceMode);
                    break;
            }

            // Texutre 0
            if (IsEnableTexture(0, surfaceMode))
            {
                GL.Arb.ActiveTexture(TextureUnit.Texture0);
                BindTexture(0);

                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)All.Replace);
            }

            // Texutre 1
            if (IsEnableTexture(1, surfaceMode))
            {
                GL.Arb.ActiveTexture(TextureUnit.Texture1);
                BindTexture(1);

                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)All.Modulate);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV_MODE, GL_COMBINE_EXT);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_COMBINE_RGB_EXT, GL_ADD_SIGNED_EXT);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_SOURCE0_RGB_EXT, GL_TEXTURE);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_SOURCE1_RGB_EXT, GL_PREVIOUS_EXT);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_OPERAND0_RGB_EXT, GL_SRC_COLOR);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_OPERAND1_RGB_EXT, GL_SRC_COLOR);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_COMBINE_ALPHA_EXT, GL_MODULATE);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_SOURCE0_ALPHA_EXT, GL_TEXTURE);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_SOURCE1_ALPHA_EXT, GL_PREVIOUS_EXT);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_OPERAND0_ALPHA_EXT, GL_SRC_ALPHA);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_OPERAND1_ALPHA_EXT, GL_SRC_ALPHA);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_RGB_SCALE_EXT, 1.0f);
            }
        }

        private bool IsEnableTexture(int nId, SurfaceMode surfaceMode)
        {
            bool result = false;
            switch (nId)
            {
                case 0:     // Stripe Texture
                    result = (surfaceMode == SurfaceMode.TextureColor) || (surfaceMode == SurfaceMode.Mixed);
                    break;
                case 1:     // 2D Texture
                    result = (surfaceMode == SurfaceMode.Image) || (surfaceMode == SurfaceMode.Mixed);
                    result &= textureImage != null && textureImage.IsValid();
                    break;
            }
            return result;
        }

        private void SetMap0(SurfaceMode surfaceMode)
        {
            float[] mat_front = new float[] { 1.0f, 1.0f, 1.0f, 0.5f };
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, mat_front);
        }

        private void SetMap1(SurfaceMode surfaceMode)
        {
            float[] mat_front = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, mat_front);
        }

        private void BindTexture(int nId)
        {
            switch (nId)
            {
                case 0:
                    TextureColor.Bind();
                    break;
                case 1:
                    textureImage.Bind();
                    break;
            }
        }
    }
}
