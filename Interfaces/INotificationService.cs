namespace InventoryManagementBlazorServer.Interfaces;

public interface INotificationService
{
    void NotifySuccess(string message);
    void NotifyError(string message);
    void NotifyWarning(string message);
    void NotifyInfo(string message);
}