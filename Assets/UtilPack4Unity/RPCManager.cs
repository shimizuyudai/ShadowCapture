using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
namespace UtilPack4Unity
{
    public class RPCManager : MonoBehaviour
    {

        public event Action<object[]> OnRaisedEvent;

        public void Call(string methodName, object[] args)
        {
            var mi = typeof(RPCManager).GetMethod(methodName);
            if (mi == null) return;
            mi.Invoke(this, args);
        }

        private void Execute(string componentType, string methodName, object[] args)
        {
            var type = Type.GetType(componentType);
            if (type == null) return;
            var objects = FindObjectsOfType(type);
            foreach (var obj in objects)
            {
                try
                {
                    var component = Convert.ChangeType(obj, type);
                    if (component == null) continue;
                    var mi = type.GetMethod(methodName);
                    mi.Invoke(component, args);
                }
                catch
                {

                }
            }
        }

        private void Send(string gameObjectName, string methodName, object[] args)
        {
            var go = GameObject.Find(gameObjectName);
            if (go == null) return;
            go.SendMessage(methodName, args);
        }

        private void Raise(object[] args)
        {
            if (OnRaisedEvent != null) OnRaisedEvent(args);
        }
    }
}
