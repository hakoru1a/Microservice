public class ApiSuccessResult<T> : ApiResult<T>
{
    public ApiSuccessResult(T data) : base(isSucceeded: true, data)
    {
    }

    public ApiSuccessResult(T data, string message) : base(isSucceeded: true, data, message)
    {
    }
}