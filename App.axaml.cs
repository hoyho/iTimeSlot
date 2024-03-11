using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using iTimeSlot.Models;
using iTimeSlot.Shared;
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
        

        
        string fileName = Global.ConfigPath;        
        Debug.WriteLine($"LoadTimeSlots from {fileName}");
        
        if (File.Exists(fileName))
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                Console.WriteLine(jsonString);
                
                var settings = JsonSerializer.Deserialize(jsonString,new JsonContext().Settings);
                Global.LoaddedSetting = settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var settings = Global.EnsureDefaultConfigFile();
                Global.LoaddedSetting = settings;
            }
        }
        else
        {
            Console.WriteLine("setting.json not found, use default time slots");
            var settings = Global.EnsureDefaultConfigFile();
            Global.LoaddedSetting = settings;

        }
        
    }

}