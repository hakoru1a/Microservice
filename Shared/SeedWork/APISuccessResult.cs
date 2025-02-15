﻿using System.Text.Json.Serialization;

public class ApiSuccessResult<T> : ApiResult<T>
{
    [JsonConstructor]
    public ApiSuccessResult(T data) : base(true, data)
    {
    }

    public ApiSuccessResult(T data, string message) : base(true, data, message)
    {
    }
}