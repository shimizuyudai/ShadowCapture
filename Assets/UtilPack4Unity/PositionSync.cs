using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PositionSync : MonoBehaviour {
    [SerializeField]
    Transform referenceTransform;

    // Update is called once per frame
    void Update () {
        if (referenceTransform == null) return;
        this.transform.position = referenceTransform.position;
	}
}
