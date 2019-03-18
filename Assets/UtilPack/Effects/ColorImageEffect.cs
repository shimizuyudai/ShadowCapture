using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorImageEffect : GrabbableImageFilter {
    [SerializeField]
    Color color = Color.white;
	
	// Update is called once per frame
	void Update () {
        material.SetColor("_Color",color);
	}
}
