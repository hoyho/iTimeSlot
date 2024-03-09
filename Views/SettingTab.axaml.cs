using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using iTimeSlot.Models;
using iTimeSlot.ViewModels;

namespace iTimeSlot.Views;

public partial class SettingTab : UserControl
{
    public SettingTab()
    {
        InitializeComponent();
    }

    private void AddTimeBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var vm = this.DataContext as MainWindowViewModel;
        if (vm == null)
        {
            Console.WriteLine("---data context is not MainWindows View Model---");
        }
        
        var toAdd = this.ToAddNud.Value;
        if (toAdd >= 1)
        {
            vm.AllTimeSlots.Add(new TimeSlot((int) toAdd));
        }
        
        int currIdx = vm.AllTimeSlots.Count -1;
        
        AllSlotsLb.ScrollIntoView(currIdx);
        AllSlotsLb.SelectedIndex = currIdx;
        
        Console.WriteLine($"{toAdd} added..");
    }
}