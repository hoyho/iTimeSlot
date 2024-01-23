using System.Text.Json;

namespace iTimeSlot.Views;

public partial class MainPage : ContentPage
{
	int count = 0;
	// TimeSpan SelectedTime = TimeSpan.FromMinutes(10);
	List<TimeSpan> allTimeSlots = new();


	public MainPage()
	{
		InitializeComponent();

		LoadTimeSlots();
	}

	private void LoadTimeSlots()
	{
		allTimeSlots.Clear();

		Console.WriteLine("LoadTimeSlots");

		string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		string fileName = Path.Combine(dir, "time_slots.json");

		if (File.Exists(fileName))
		{
			try
			{
				string jsonString = File.ReadAllText(fileName);
				Console.WriteLine(jsonString);
				List<TimeSpan> slots = JsonSerializer.Deserialize<List<TimeSpan>>(jsonString);
				allTimeSlots = slots;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		else
		{
			Console.WriteLine("setting.json not found, use default time slots");
			allTimeSlots = iTimeSlot.Shared.DefaultConfig.SysTimeSlots;
		}

		pickerCurrentTimeSlot.ItemsSource = allTimeSlots;
		pickerCurrentTimeSlot.SelectedIndex = 0; //todo: load from setting
	}

	private async void OnStartClicked(object sender, EventArgs e)
	{

		var tm = Shared.Global.MyTimer;
		var selected = allTimeSlots[pickerCurrentTimeSlot.SelectedIndex];
		tm.Init(DateTime.Now, selected, progressBar.ProgressTo);
		//update to full before start which will be reset to 0
		await progressBar.ProgressTo(1, 0, Easing.Default);
		progressBar.IsVisible = true;

		tm.Start();
		pickerCurrentTimeSlot.IsEnabled = false;
		StartBtn.IsEnabled = false;
	}

	private async void OnCancelClicked(object sender, EventArgs e)
	{
		var tm = Shared.Global.MyTimer;
		tm.Stop();
		await progressBar.ProgressTo(0, 0, Easing.Default);
		progressBar.IsVisible = false;
		StartBtn.IsEnabled = true;
		pickerCurrentTimeSlot.IsEnabled = true;

	}
}

