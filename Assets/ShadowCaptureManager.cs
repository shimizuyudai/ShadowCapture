using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

public class ShadowCaptureManager : MonoBehaviour
{
    [SerializeField]
    TextureHolderBase[] textureHolders;
    
    [SerializeField]
    DifferenceImageFilter differenceImageFilter;
    [SerializeField]
    HeatmapFilter heatmapFilter;
    [SerializeField]
    float delay;


    [Header("Debug")]
    [SerializeField]
    KeyCode captureKey;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            Capture();
        }
        
    }

    void Capture()
    {
        heatmapFilter.Clear();
        differenceImageFilter.Capture();
    }

    IEnumerator InitializeRoutine()
    {
        yield return new WaitForSeconds(delay);
        Capture();

    }
}
