using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceVisualizer : MonoBehaviour {
    [SerializeField]
    Transform target;
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    float maxDistance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.SetFloat("_MaxDistance", maxDistance);
        renderer.material.SetVector("_TargetPosition", target.position);
	}
}
