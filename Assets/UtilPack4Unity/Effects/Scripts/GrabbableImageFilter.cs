using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class GrabbableImageFilter : ImageEffectApplier
    {
        [SerializeField]
        protected int id;
        public int Id
        {
            get
            {
                return this.id;
            }
        }
        [SerializeField]
        public bool IsSelfFilter;

        public delegate void FilterDeletgate(RenderTexture destionation);
        public event FilterDeletgate OnFilteredEvent;

        public virtual void Filter(Texture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, material);
            OnFiltered(destination);
        }

        protected virtual void OnFiltered(RenderTexture destination)
        {
            OnFilteredEvent?.Invoke(destination);
        }

        public virtual void Through(Texture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination);
            OnFiltered(destination);
        }

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (IsSelfFilter)
            {
                Filter(source, destination);
            }
            else
            {
                Through(source, destination);
            }
        }
    }
}
