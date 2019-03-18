using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
[RequireComponent(typeof(TextureUtilBehaviour))]
public class TextureHolderExporter : MonoBehaviour
{
    [SerializeField]
    string prefix;
    [SerializeField]
    bool useDateTime;
    [SerializeField]
    string directoryName;
    [SerializeField]
    TextureHolderBase textureHolder;
    Texture2D texture;
    RenderTexture renderTexture;
    [SerializeField]
    TextureUtilBehaviour textureUtilBehaviour;
    [SerializeField]
    TextureUtilBehaviour.ImageType imageType;

    [SerializeField]
    KeyCode exportKey;

    private void Awake()
    {
        textureHolder.ChangeTextureEvent += TextureHolder_ChangeTextureEvent;
    }

    private void TextureHolder_ChangeTextureEvent(Texture texture)
    {
        textureUtilBehaviour.SecureTexture(texture, this.texture);
        textureUtilBehaviour.SecureTexture(texture, this.renderTexture);
    }

    private void Update()
    {
        if (Input.GetKeyDown(exportKey))
        {
            Export();
        }
    }

    public void Export()
    {
        var name = (string.IsNullOrEmpty(prefix) ? string.Empty : prefix + "_") + (useDateTime ? TypeUtils.Format.DateTime2String(DateTime.Now) : string.Empty);
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("file name is empty.");
        }
        var fileName = name  + "." + imageType.ToString();
        var path = Path.Combine(Application.streamingAssetsPath, directoryName, fileName);
        textureUtilBehaviour.SaveTexture(path, imageType, textureHolder.GetTexture(), texture, renderTexture);
    }
}
