using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class TextureUtils
    {

        public static void RenderTexture2Texture2D(RenderTexture renderTexture, Texture2D texture2D)
        {
            var rt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = rt;
        }


        public static void Texture2RenderTexture(Texture texture, RenderTexture rt)
        {
            Graphics.Blit(texture, rt);
        }

        public static void Texture2Texture2D(Texture texture, Texture2D texture2D, RenderTexture rt)
        {
            Texture2RenderTexture(texture, rt);
            RenderTexture2Texture2D(rt, texture2D);
        }

        public static Texture2D CreateTexture2DFromRenderTexture(RenderTexture renderTexture)
        {
            TextureFormat textureFormat;
            switch (renderTexture.format)
            {
                case RenderTextureFormat.ARGB32:
                    textureFormat = TextureFormat.ARGB32;
                    break;

                case RenderTextureFormat.ARGBHalf:
                    textureFormat = TextureFormat.RGBAHalf;
                    break;

                case RenderTextureFormat.ARGBFloat:
                    textureFormat = TextureFormat.RGBAFloat;
                    break;

                case RenderTextureFormat.RGHalf:

                    textureFormat = TextureFormat.RGHalf;

                    break;

                case RenderTextureFormat.RHalf:

                    textureFormat = TextureFormat.RHalf;

                    break;

                default:
                    textureFormat = TextureFormat.ARGB32;
                    break;
            }
            var texture = new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false);
            RenderTexture2Texture2D(renderTexture, texture);
            return texture;
        }

        public static void PingPongTextures(Texture[] textures)
        {
            var temp = textures[0];
            textures[0] = textures[1];
            textures[1] = temp;
        }

        public static void Resize(Texture fromTex, Texture2D toTex)
        {
            var rt = RenderTexture.GetTemporary(toTex.width, toTex.height);
            Graphics.Blit(fromTex, rt);

            var pre = RenderTexture.active;
            RenderTexture.active = rt;
            toTex.ReadPixels(new Rect(0, 0, toTex.width, toTex.height), 0, 0);
            toTex.Apply();
            RenderTexture.active = pre;
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}