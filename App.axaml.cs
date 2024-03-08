using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using iTimeSlot.ViewModels;
using iTimeSlot.Views;

namespace iTimeSlot;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        LoadTimeSlots();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //bind App.axaml to ApplicationViewModel 
            this.DataContext = new ApplicationViewModel();
            
            desktop.MainWindow = new MainWindow
            {
                //DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    //Load time_slots.json or default config to shared instance
    private void LoadTimeSlots()
    {
        
        Debug.WriteLine("LoadTimeSlots");

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string fileName = Path.Combine(dir, "time_slots.json");

        if (File.Exists(fileName))
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                Console.WriteLine(jsonString);
                List<TimeSpan> slots = JsonSerializer.Deserialize<List<TimeSpan>>(jsonString);
                Shared.Global.ExistTimeSpans = slots;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("setting.json not found, use default time slots");
            Shared.Global.ExistTimeSpans = iTimeSlot.Shared.DefaultConfig.SysTimeSlots;
        }
        
    }

}