using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public static T Instance
    {
		get
        {
            if(_instance == null)
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

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	void OnDestroy ()
    {
        Destroy(gameObject);
    }
}