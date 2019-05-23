using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureUtilBehaviour : MonoBehaviour
{
    public enum ImageType
    {
        JPG,
        PNG,
        TGA,
        EXR
    }

    public Texture2D SecureTexture(Texture referenceTexture, Texture2D texture, TextureFormat format = TextureFormat.RGBA32, bool mipChain = false)
    {
        if (texture == null)
        {
            texture = new Texture2D(referenceTexture.width, referenceTexture.height, format, mipChain);
        }
        else if (texture.width != referenceTexture.width || texture.height != referenceTexture.height)
        {
            DestroyImmediate(texture);
            texture = null;
            texture = new Texture2D(referenceTexture.width, referenceTexture.height, format, mipChain);
        }
        return texture;
    }

    public RenderTexture SecureTexture(Texture referenceTexture, RenderTexture texture, int depth = 32, RenderTextureFormat format = RenderTextureFormat.ARGB32)
    {
        if (texture == null)
        {
            texture = new RenderTexture(referenceTexture.width, referenceTexture.height, depth, format);
        }
        else if (texture.width != referenceTexture.width || texture.height != referenceTexture.height)
        {
            texture.Release();
            DestroyImmediate(texture);
            texture = null;
            texture = new RenderTexture(texture.width, texture.height, depth, format);
        }
        return texture;
    }

    public void SaveTexture(string filePath, ImageType imageType, Texture texture, Texture2D texture2D, RenderTexture renderTexture)
    {
        texture2D = SecureTexture(texture, texture2D);
        renderTexture = SecureTexture(texture, renderTexture);
        TextureUtils.Texture2Texture2D(texture, texture2D, renderTexture);
        switch (imageType)
        {
            case ImageType.JPG:
                {
                    var bytes = texture2D.EncodeToJPG();
                    File.WriteAllBytes(filePath, bytes);
                }
                break;
            case ImageType.PNG:
                {
                    var bytes = texture2D.EncodeToJPG();
                    File.WriteAllBytes(filePath, bytes);
                }
                break;
            case ImageType.TGA:
                {
                    var bytes = texture2D.EncodeToJPG();
                    File.WriteAllBytes(filePath, bytes);
                }
                break;
            case ImageType.EXR:
                {
                    var bytes = texture2D.EncodeToEXR();
                    File.WriteAllBytes(filePath, bytes);
                }
                break;
        }
    }
}
