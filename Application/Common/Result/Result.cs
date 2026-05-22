namespace Application.Common.Result;

public abstract class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public List<string> Errors { get; }

    protected Result(bool isSuccess, string message, List<string> errors)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = errors;
    }

    public static Result<T> Success<T>(T data, string message = "Operation completed successfully")
        => new Result<T>(true, data, message, new List<string>());

    public static Result<T> Failure<T>(string message, List<string>? errors = null)
        => new Result<T>(false, default!, message, errors ?? new List<string>());

    public static Result Success(string message = "Operation completed successfully")
        => new EmptyResult(true, message, new List<string>());

    public static Result Failure(string message, List<string>? errors = null)
        => new EmptyResult(false, message, errors ?? new List<string>());
}

public class Result<T> : Result
{
    public T? Data { get; }

    public Result(bool isSuccess, T? data, string message, List<string> errors)
        : base(isSuccess, message, errors)
    {
        Data = data;
    }
}

public class EmptyResult : Result
{
    public EmptyResult(bool isSuccess, string message, List<string> errors)
        : base(isSuccess, message, errors)
    {
    }
}
