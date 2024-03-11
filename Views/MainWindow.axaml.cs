using System.ComponentModel;
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
        
        Closing += OnClosing;
    }
    
    private void OnClosing(object sender, CancelEventArgs e)
    {
        e.Cancel = vm.CloseWithoutExit;
        Hide();
    }
    

}