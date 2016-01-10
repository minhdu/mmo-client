using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesManager : Singleton<ResourcesManager> {

    private Dictionary<string, Object> caches = new Dictionary<string, Object>();

    public void LoadResource (string path, System.Action<Object> onLoaded)
    {
        Object res = null;
        if (caches.TryGetValue(path, out res))
        {
            onLoaded(res);
        }
        else
        {
            StartCoroutine(LoadResourceAsync(path, result => { onLoaded(result); }));
        }
    }

    IEnumerator LoadResourceAsync (string path, System.Action<Object> onLoaded)
    {
        ResourceRequest request = Resources.LoadAsync(path);
        yield return request;
        onLoaded(request.asset);
        caches.Add(path, request.asset);
    }
}
