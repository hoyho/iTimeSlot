using System;
using System.Collections.ObjectModel;
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

    private void BtnAddTime_OnClick(object? sender, RoutedEventArgs e)
    {
        var toAdd = this.UpDownToAdd.Value;
        if (toAdd > 1)
        {
            Shared.Global.ExistTimeSpans.Add(TimeSpan.FromMinutes((int) toAdd));
        }

        LbSlots.SelectedIndex = Shared.Global.ExistTimeSpans.Count-1;
        //LbSlots.ScrollIntoView(Shared.Global.ExistTimeSpans.Count-1);
        
        Console.WriteLine("add..");
    }
}