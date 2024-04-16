using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using iTimeSlot.Views;

namespace iTimeSlot.ViewModels;

public partial class ApplicationViewModel : ViewModelBase
{


    [RelayCommand]
    private static void ShowWindow()
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
    private static void Quit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
        {
            application.Shutdown();
        }
    }


    [RelayCommand]
    private static async void About()
    {
        AboutDialog AboutDialogWindow = new AboutDialog();
        var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        AboutDialogWindow = new AboutDialog();
        
        ShowWindow();
        AboutDialogWindow.Show();
        // await AboutDialogWindow.ShowDialog(mainWindow);
    }
}