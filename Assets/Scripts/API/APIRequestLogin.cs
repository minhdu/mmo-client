using UnityEngine;
using System.Collections;
using APIMessage;

public class APIRequestLogin : APIRequest
{
    public APIRequestLogin(UserInfo userInfo)
    {
        RequestURL = APIDefine.API_LOGIN;
        SubmitData = UserInfo.SerializeToBytes(userInfo);
    }
}
