using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureHolderBase : MonoBehaviour
{

    public virtual Texture GetTexture()
    {
        return null;
    }

    public virtual void SetTexture(Texture texture)
    {
        if (ChangeTextureEvent != null) ChangeTextureEvent(texture);
    }

    public virtual void RefreshTexture()
    {
        if (RefreshTextureEvent != null) RefreshTextureEvent();
    }

    public delegate void OnChanged(Texture texture);
    public event OnChanged ChangeTextureEvent;
    public delegate void OnRefresh();
    public event OnRefresh RefreshTextureEvent;
}

