namespace Server.BlazorUI.Components;

public interface IErrorComponent
{
    void ShowError(string title, string message);
    void ShowError(string message);
}