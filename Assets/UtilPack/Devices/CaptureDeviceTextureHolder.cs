﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureDeviceTextureHolder : TextureHolderBase
{
    public event Action OnAvailable;

    protected virtual void Available()
    {
        OnAvailable?.Invoke();
    }
}
