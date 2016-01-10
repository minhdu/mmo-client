using UnityEngine;

public class NetSingleton<T> : Photon.MonoBehaviour where T : Photon.MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
            }
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }
}