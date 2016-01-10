using UnityEngine;
using System.Collections;
using APIMessage;

public class APIResponseLogin : APIResponse
{
    public APIResponseLogin()
    { 

    }

    public APIResponseLogin(APIResponse response)
    {
        Cast(response);
    }
}
