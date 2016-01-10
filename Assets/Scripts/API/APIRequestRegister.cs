using UnityEngine;
using System.Collections;
using APIMessage;

public class APIRequestRegister : APIRequest {

    public APIRequestRegister (UserInfo user)
    {
        RequestURL = APIDefine.API_REGISTER;
        SubmitData = UserInfo.SerializeToBytes(user);
    }
}
