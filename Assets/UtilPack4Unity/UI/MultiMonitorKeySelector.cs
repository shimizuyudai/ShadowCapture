using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class MultiMonitorKeySelector : MonoBehaviour
    {
        [SerializeField]
        MultiMonitorManager multiMonitorManager;

        [SerializeField]
        KeyCode incrementKey, decrementKey;

        int index;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(incrementKey))
            {
                Increment();
            }
            else if (Input.GetKeyDown(incrementKey))
            {
                Decrement();
            }
        }

        void Increment()
        {
            index++;
            if (index >= multiMonitorManager.MonitorViews.Count)
            {
                index = 0;
            }
            multiMonitorManager.OnSelected(multiMonitorManager.MonitorViews[index]);
        }

        void Decrement()
        {
            index--;
            if (index < 0)
            {
                index = multiMonitorManager.MonitorViews.Count - 1;
            }
            multiMonitorManager.OnSelected(multiMonitorManager.MonitorViews[index]);
        }
    }
}
