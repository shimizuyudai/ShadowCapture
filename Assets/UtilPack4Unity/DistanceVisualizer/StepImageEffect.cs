using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    [ExecuteInEditMode]
    public class StepImageEffect : ImageEffectApplier
    {
        [SerializeField]
        int step;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            material.SetInt("_Step", step);
        }
    }
}