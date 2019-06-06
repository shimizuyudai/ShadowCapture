using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class Bump2NormalImageFilter : GrabbableImageFilter
    {
        [SerializeField]
        float bumpRate = 1f;
        [SerializeField]
        float normalRate = 1f;

        private void Update()
        {
            material.SetFloat("_BumpRate", bumpRate);
            material.SetFloat("_NormalRate", normalRate);
        }
    }
}