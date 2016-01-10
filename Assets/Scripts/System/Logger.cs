using UnityEngine;
using System.Collections;

public class Logger {

	static public void Log (object message)
    {
#if UNITY_DEBUG
        Debug.Log(message);
#endif
    }

    static public void LogError (object message)
    {
#if UNITY_DEBUG
        Debug.LogError(message);
#endif
    }

    static public void LogWarning(object message)
    {
#if UNITY_DEBUG
        Debug.LogWarning(message);
#endif
    }
}
