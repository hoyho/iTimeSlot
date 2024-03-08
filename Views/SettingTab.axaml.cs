using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using iTimeSlot.ViewModels;

namespace iTimeSlot.Views;

public partial class SettingTab : UserControl
{
    private SettingTabViewModel vm;
    
    public SettingTab()
    {
        InitializeComponent();
        vm = new SettingTabViewModel();
        this.DataContext = vm;
    }

    private void AddTimeBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var toAdd = this.ToAddNud.Value;
        if (toAdd > 1)
        {
            Shared.Global.ExistTimeSpans.Add(TimeSpan.FromMinutes((int) toAdd));
        }

        //AllSlotsLb.SelectedIndex = Shared.Global.ExistTimeSpans.Count-1;
        AllSlotsLb.ScrollIntoView(Shared.Global.ExistTimeSpans.Last());
        
        Console.WriteLine("add..");
    }
}