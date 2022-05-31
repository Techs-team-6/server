using BlazorContextMenu;

namespace Server.BlazorUI.Pages.Tools;

public static class ItemClickEventArgsExtension
{
    public static T GetTarget<T>(this ItemClickEventArgs e)
    {
        var container = e.ContextMenuTrigger.ChildContent.Target!;
        return (T)container.GetType().GetFields()[0].GetValue(container)!;
    }
}