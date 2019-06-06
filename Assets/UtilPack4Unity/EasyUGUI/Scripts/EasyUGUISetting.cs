using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EasyUGUISetting : ScriptableObject
{
    public event Action InitializedEvent;
    public event Action UpdatedEvent;
    public event Action SavedEvent;
    public event Action ReloadedEvent;

    public void OnReloaded()
    {
        ReloadedEvent?.Invoke();
    }
    
    public void OnInitialized()
    {
        InitializedEvent?.Invoke();
    }

    public void OnUpdated()
    {
        UpdatedEvent?.Invoke();
    }

    public void OnSaved()
    {
        SavedEvent?.Invoke();
    }
}
