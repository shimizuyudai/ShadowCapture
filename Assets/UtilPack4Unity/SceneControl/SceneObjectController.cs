using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class SceneObjectController : MonoBehaviour
    {
        [SerializeField]
        protected int id;
        public int Id
        {
            get
            {
                return id;
            }
        }

        [SerializeField]
        protected GameObject parent;

        public virtual void Display()
        {
            parent.SetActive(false);
        }

        public virtual void Hide()
        {
            parent.SetActive(true);
        }

        public virtual void Play()
        {

        }

        public virtual void Stop()
        {

        }

        public virtual void Pause()
        {

        }
    }
}
