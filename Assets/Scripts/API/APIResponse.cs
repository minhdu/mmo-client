using UnityEngine;
using System.Collections;

public enum ResponseStatus
{
    OK = 200,
    UNAUTHORIZE = 203,
    FILE_NOT_FOUND = 404,
    CONFLICT = 409,
    INTERNAL_SERVER_ERROR = 500,
    BAD_REQUEST = 502
}

public class APIResponse {

    public ResponseStatus Status { get; set; }
    public string Message { get; set; }
    public byte[] ResponseBinaries { get; set; }

    public APIResponse ()
    {

    }

    public void Cast (APIResponse response)
    {
        this.Status = response.Status;
        this.Message = response.Message;
        this.ResponseBinaries = response.ResponseBinaries;
    }
}
