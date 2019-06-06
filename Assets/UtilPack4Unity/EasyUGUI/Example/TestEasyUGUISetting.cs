using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUGUI;

[CreateAssetMenu(fileName = "TestEasyUGUISetting.asset", menuName = "Custom/Create TestEasyUGUISetting")]
public class TestEasyUGUISetting : EasyUGUISetting
{
    [Range(0,10)]
    public float foo;
    [SerializeField]
    [Range(0,1)]
    private int bar;

    public enum Type
    {
        AAAA,
        BBBB,
        CCCC
    }
    [SerializeField]
    Type type;
    
    [SerializeField]
    [MultilineText]
    private string str;
}
