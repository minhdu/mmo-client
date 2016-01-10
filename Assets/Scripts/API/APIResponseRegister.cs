using UnityEngine;
using System.Collections;
using APIMessage;

public class APIResponseRegister : APIResponse {

    public APIResponseRegister ()
    {

    }

    public APIResponseRegister (APIResponse response)
    {
        Cast(response);
    }
}
