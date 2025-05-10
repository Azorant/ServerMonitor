namespace ServerMonitor.Models;

public class RestResponse<T>(bool isSuccess, T response, Exception? exception = null)
{
    public T Response { get; set; } = response;
    public bool IsSuccess { get; set; } = isSuccess;
    public Exception? Exception { get; set; } = exception;
}