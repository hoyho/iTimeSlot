using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace iTimeSlot.Views;

public partial class SettingTab : UserControl
{
    public SettingTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}