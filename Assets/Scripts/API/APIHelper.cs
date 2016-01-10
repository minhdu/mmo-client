using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class APIHelper : MonoBehaviour {

    static public void CallAPI(APIRequest request, Action<APIResponse> onResponse)
    {
        GameObject apiCarrier = new GameObject();
        apiCarrier.name = request.GetType().ToString();
        APIHelper api = apiCarrier.AddComponent<APIHelper>();
        api.Request(request, response => { onResponse(response); });
    }

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Request (APIRequest request, Action<APIResponse> onResponse)
    {
        StartCoroutine(WWWRequest(request.RequestURL, request.SubmitData, response =>
        {
            onResponse(response);
        }));
    }

    IEnumerator WWWRequest (string url, byte[] data, Action<APIResponse> onResponse)
    {
        Dictionary<string, string> headers = BuildHeaders();
        url = BuildURL(url);

#if UNITY_DEBUG
        Logger.Log(string.Format("<b>Request: </b><color=blue>{0}</color>, Data lenght: {1}", url, data.Length));
#endif

        WWW w = new WWW(url, data, headers);
        yield return w;

        APIResponse response = new APIResponse();
        if (string.IsNullOrEmpty(w.error))
        {
            response.Status = ResponseStatus.OK;
            response.ResponseBinaries = w.bytes;
        }
        else
        {
            try
            {
                string strCode = Regex.Replace(w.error, @"[^0-9]+", "");
                response.Status = (ResponseStatus)int.Parse(strCode);
                Logger.LogError(w.error);
            }
            catch
            {
                Logger.LogError("Unexpected error!");
            }
            
        }

        onResponse(response);
        Destroy(gameObject);
    }

    Dictionary<string, string> BuildHeaders ()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add(APIDefine.HEADER_CONTENT_TYPE, APIDefine.HEADER_VAL_BINARY);
        headers.Add(APIDefine.HEADER_APP_VERSION, APIDefine.HEADER_VAL_APP_VERSION);
        headers.Add(APIDefine.HEADER_DEVICE_TYPE, APIDefine.HEADER_VAL_DEVICE_TYPE);
        headers.Add(APIDefine.HEADER_APP_ID, APIDefine.HEADER_VAL_APPID);
        headers.Add(APIDefine.HEADER_SESSION, "null");
        return headers;
    }

    string BuildURL (string router)
    {
        return string.Format("{0}://{1}{2}", APIDefine.PROTOCOL, APIDefine.HOST_NAME, router);
    }
}