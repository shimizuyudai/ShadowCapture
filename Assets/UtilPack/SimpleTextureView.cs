using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTextureView : MonoBehaviour {
    [SerializeField]
    TextureHolderBase textureHolder;
    [SerializeField]
    Renderer renderer;

	void Start () {
		
	}
	
	void Update () {
        renderer.material.mainTexture = textureHolder.GetTexture();
	}
}
