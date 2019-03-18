using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace Persistence
{
    public class Exportable
    {
        public virtual Exportable Export()
        {
            return this;
        }
    }   
}

