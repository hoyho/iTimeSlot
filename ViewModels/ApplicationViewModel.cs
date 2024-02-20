using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using iTimeSlot_avalonia.Views;

namespace iTimeSlot_avalonia.ViewModels;

public partial class ApplicationViewModel:ViewModelBase
{
    
        
    [RelayCommand]
    private void ShowWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWin = desktop.MainWindow;
            if (mainWin != null)
            {
                mainWin.WindowState = WindowState.Normal;
                mainWin.Activate();
                mainWin.Show();
                mainWin.BringIntoView();
                mainWin.Focus();    
            }
            else
            {
                Console.WriteLine("main windows was not inited");
            }

        }
    }


    [RelayCommand]
    private static void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
        {
            application.Shutdown();
        }
    }
}