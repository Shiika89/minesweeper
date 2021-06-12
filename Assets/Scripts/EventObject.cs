using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnStageClear += StageClear;   
    }
    private void OnDisable()
    {
        EventManager.OnStageClear -= StageClear;
    }
    public virtual void StageClear()
    {

    }
}
