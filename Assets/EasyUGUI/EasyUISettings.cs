using UnityEngine;
using System;

[CreateAssetMenu( menuName = "Custom/Create EasyUISettings", fileName = "EasyUISettings.asset" )]
public class EasyUISettings : ScriptableObject
{
    [SerializeField]
    GameObject[] elementPrefabs;
    [SerializeField]
    GameObject spacePrefab;
}

