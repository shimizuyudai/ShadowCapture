using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaptureSettingsManager : MonoBehaviour
{
    [SerializeField]
    GameObject uiParent;
    [SerializeField]
    UIDecimalValueController[] valueControllers;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var valueController in valueControllers)
        {
            valueController.ValueChangedEvent += ValueController_ValueChangedEvent;
        }
    }

    private void ValueController_ValueChangedEvent(UIDecimalValueController controller)
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
