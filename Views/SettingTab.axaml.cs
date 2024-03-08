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
        if (toAdd >= 1)
        {
            Shared.Global.ExistTimeSpans.Add(TimeSpan.FromMinutes((int) toAdd));
        }

        int currIdx = Shared.Global.ExistTimeSpans.Count-1;
        for (int i = 0; i < Shared.Global.ExistTimeSpans.Count; i++)
        {
            if (Shared.Global.ExistTimeSpans[i].Minutes == toAdd)
            {
                currIdx = i;
                break;
            }
        }

        AllSlotsLb.ScrollIntoView(currIdx);
        AllSlotsLb.SelectedIndex = currIdx;
        
        Console.WriteLine("add..");
    }
}