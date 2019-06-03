using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaptureSettingsManager : MonoBehaviour
{
    [SerializeField]
    GameObject uiParent;
    [SerializeField]
    EasyUGUIControl[] valueControllers;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedButton()
    {
        uiParent.SetActive(!uiParent.activeSelf);
    }
}
