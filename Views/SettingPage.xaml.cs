using System.Text.Json;
using System.Text.Json.Serialization;
using ObjCRuntime;

namespace NoCloud.Views;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();
	}

	private async void Load_Clicked(object sender, EventArgs e)
	{
		Console.WriteLine("Load_Clicked");
		string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		string fileName = Path.Combine(dir, "setting.json");

		if (File.Exists(fileName))
		{
			try
			{
				string jsonString = File.ReadAllText(fileName);
				Console.WriteLine(jsonString);
				Models.Setting setting = JsonSerializer.Deserialize<Models.Setting>(jsonString);
				Console.WriteLine(setting?.URL);
				BindingContext = setting;
				Console.WriteLine("BindingContext updated");
				await NoCloud.Views.Common.ShowToastAsync("BindingContext updated from setting.json");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

	}
	private async void Save_Clicked(object sender, EventArgs e)
	{
		Console.WriteLine("Save_Clicked");
		//print current working directory
		//get home directory
		string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		// Console.WriteLine(Directory.GetCurrentDirectory());
		Console.WriteLine(BindingContext);

		if (BindingContext is Models.Setting s)
		{

			// Save the setting to a file.
			string fileName = Path.Combine(dir, "setting.json");
			try
			{
				string jsonString = JsonSerializer.Serialize(s);
				Console.WriteLine("config path: " + fileName);
				File.WriteAllText(fileName, jsonString);
				Console.WriteLine(File.ReadAllText(fileName));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
			await NoCloud.Views.Common.ShowToastAsync("Setting saved");
			// // Navigate to the specified URL in the system browser.
			// await Launcher.Default.OpenAsync(s.URL);

		}
	}

}
