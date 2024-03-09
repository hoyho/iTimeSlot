using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using iTimeSlot.Models;
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
        var ls =  new ObservableCollection<TimeSlot>();
        foreach (var t in Shared.Global.ExistTimeSpans)
        {
         ls.Add(new TimeSlot(t));   
        }
        vm.AllTimeSlots = ls;
        this.DataContext = vm;
    }

}