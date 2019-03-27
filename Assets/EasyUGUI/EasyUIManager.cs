using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity.EasyUI
{
    public class EasyUIManager : MonoBehaviour
    {
        [SerializeField]
        GameObject panelPrefab;

        // Start is called before the first frame update
        void Start()
        {
            
            var a = "hello";
            var b = a;
            b = "test";
            print("----------------start---------------");
            print(a);
            print(b);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

