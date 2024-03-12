using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using iTimeSlot.ViewModels;

namespace iTimeSlot.Views;

public partial class SettingTab : UserControl
{
    public SettingTab()
    {
        InitializeComponent();
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if(DataContext is MainWindowViewModel ctx)
        {
            ctx.SettingTabNotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this)!);
        }
    }
}