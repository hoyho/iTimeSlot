using Avalonia.Controls;
using iTimeSlot.Shared;
using iTimeSlot.ViewModels;

namespace iTimeSlot.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel vm;

    public MainWindow()
    {
        InitializeComponent();

        vm = new MainWindowViewModel();
        vm.Fill(Global.LoaddedSetting);
        this.DataContext = vm;
    }

}