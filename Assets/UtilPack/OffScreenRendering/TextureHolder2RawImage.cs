using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureHolder2RawImage : MonoBehaviour
{
    [SerializeField]
    TextureHolderBase textureHolder;
    [SerializeField]
    RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rawImage.texture = textureHolder.GetTexture();
    }
}
