using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //An event which is accessible in the inspector
    public event Action<Target> onDestroyed;

    //Simple onDestroy invoke the onDestroy method
    private void onDestroy()
    {
        onDestroyed?.Invoke(this);
    }
}
