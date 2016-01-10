using UnityEngine;
using System.Collections;

public class APIDefine {

    //Basic infomations
    public const string PROTOCOL = "http";
    public const string PORT = "";

#if DEV
    public const string HOST_NAME = "ekgames.cloudapp.net/server4";
    //public const string HOST_NAME = "localhost:8080/dev";
#elif BETA
    public const string HOST_NAME = "ekgames.cloudapp.net/beta";
#elif RELEASE
    public const string HOST_NAME = "ekgames.cloudapp.net/reslese";
#else
    public const string HOST_NAME = "ekgames.cloudapp.net/server4";
#endif

    public const string HEADER_CONTENT_TYPE = "Content-Type";
    public const string HEADER_APP_VERSION = "x-appversion";
    public const string HEADER_DEVICE_TYPE = "x-devicetype";
    public const string HEADER_APP_ID = "x-appid";
    public const string HEADER_SESSION = "x-sessionid";

    public const string HEADER_VAL_BINARY = "application/octet-stream";
    public const string HEADER_VAL_JSON = "application/json";

#if UNITY_IOS
    public const string HEADER_VAL_APPID = "com.ekgames.newmmo";
#elif UNITY_ANDROID
    public const string HEADER_VAL_APPID = "com.ekgames.newmmo";
#else
    public const string HEADER_VAL_APPID = "NEWMMO-PC";
#endif


#if DEV
    public const string HEADER_VAL_APP_VERSION = "1.0.0";
#elif BETA
    public const string HEADER_VAL_APP_VERSION = "1.0.0";
#elif RELEASE
    public const string HEADER_VAL_APP_VERSION = "1.0.0";
#else
    public const string HEADER_VAL_APP_VERSION = "1.0.0";
#endif

#if UNITY_IOS
    public const string HEADER_VAL_DEVICE_TYPE = "IOS";
#elif UNITY_ANDROID
    public const string HEADER_VAL_DEVICE_TYPE = "ADR";
#else
    public const string HEADER_VAL_DEVICE_TYPE = "XPC";
#endif

    //API routers
    public const string API_REGISTER = "/register";
    public const string API_LOGIN = "/login";
}
