using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdImageFilter : GrabbableImageFilter {
    [SerializeField]
    float threshold;

	void Update () {
        material.SetFloat("_Threshold", threshold);
	}
}
