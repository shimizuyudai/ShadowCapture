using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureHolder2Renderer : MonoBehaviour {
    [SerializeField]
    TextureHolderBase textureHolder;
    [SerializeField]
    Renderer renderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.mainTexture = textureHolder.GetTexture();
	}
}
