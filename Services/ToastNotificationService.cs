using InventoryManagementBlazorServer.Interfaces;
using Microsoft.JSInterop;

namespace InventoryManagementBlazorServer.Services;

public class ToastNotificationService(IJSRuntime jsRuntime) : INotificationService
{
    public async void NotifySuccess(string message)
    {
        await jsRuntime.InvokeVoidAsync("window.toastr.success", message);
    }

    public async void NotifyError(string message)
    {
        await jsRuntime.InvokeVoidAsync("window.toastr.error", message);
    }

    public async void NotifyWarning(string message)
    {
        await jsRuntime.InvokeVoidAsync("window.toastr.warning", message);
    }

    public async void NotifyInfo(string message)
    {
        await jsRuntime.InvokeVoidAsync("window.toastr.info", message);
    }
}