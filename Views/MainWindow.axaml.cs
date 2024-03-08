using System;
using Avalonia.Controls;
using iTimeSlot.Foundation;
using iTimeSlot.ViewModels;

namespace iTimeSlot.Views;

public partial class MainWindow : Window
{
    //List<TimeSpan> _allTimeSlots = new();
    private MainWindowViewModel vm;

    public MainWindow()
    {
        InitializeComponent();

        vm = new MainWindowViewModel();
        this.DataContext = vm;
        //update index after binding
    }

}