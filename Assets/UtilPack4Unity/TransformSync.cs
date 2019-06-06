using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : MonoBehaviour
{
    [SerializeField]
    Transform referenceTransform;
    [SerializeField]
    bool syncPosition, syncRotation, syncScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (syncPosition)
        {
            this.transform.localPosition = referenceTransform.position;
        }
        if (syncRotation)
        {
            this.transform.localRotation = referenceTransform.rotation;
        }
        if (syncScale)
        {
            this.transform.localScale = referenceTransform.lossyScale;
        }
    }
}
