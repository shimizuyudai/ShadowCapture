using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gameObjects;
    [SerializeField]
    KeyCode incrementKey, decrementKey;

    int index;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(incrementKey))
        {
            increment();
        }
        if (Input.GetKeyDown(decrementKey))
        {
            decrement();
        }
    }

    void increment()
    {
        index++;
        if (index >= gameObjects.Count)
        {
            index = 0;
        }
        Activate(index);
    }


    void decrement()
    {
        index--;
        if (index < 0)
        {
            index = gameObjects.Count - 1;
        }
        Activate(index);
    }

    private void Activate(int index)
    {
        for (var i = 0; i < gameObjects.Count; i++)
        {
            if (i == index)
            {
                gameObjects[i].SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }
    }
}
