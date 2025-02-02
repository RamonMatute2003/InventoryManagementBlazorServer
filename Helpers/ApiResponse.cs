namespace InventoryManagementBlazorServer.Helpers;

public class ApiResponse<T>(int code, string message, T? body = default)
{
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
    public T? Body { get; set; } = body;

    public bool IsSuccess => Code >= 200 && Code < 300;
}