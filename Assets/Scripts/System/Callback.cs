using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Callback
{
    private List<Action<object[]>> m_Callbacks = new List<Action<object[]>>();
    public List<Action<object[]>> Callbacks
    {
        get { return m_Callbacks; }
        set { m_Callbacks = value; }
    }

    public Callback (params Action<object[]>[] callbacks)
    {
        if (callbacks == null || callbacks.Length == 0)
            return;
        for(int i=0; i<callbacks.Length; i++)
        {
            Callbacks.Add(callbacks[i]);
        }
    }
}