using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace UtilPack4Unity
{
    public class Exportable
    {
        public virtual Exportable Export()
        {
            return this;
        }
    }   
}

