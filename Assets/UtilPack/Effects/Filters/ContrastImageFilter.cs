using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrastImageFilter : GrabbableImageFilter {
    [SerializeField]
    float contrast;

	void Update () {
        material.SetFloat("_Contrast", contrast);
	}
}
