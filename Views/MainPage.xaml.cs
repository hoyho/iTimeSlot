using System.Text.Json;
using iTimeSlot.Services;

namespace iTimeSlot.Views;

public partial class MainPage : ContentPage
{
	int count = 0;
	// TimeSpan SelectedTime = TimeSpan.FromMinutes(10);
	List<TimeSpan> allTimeSlots = new();

	static bool isSetup = false;


	public MainPage()
	{
		InitializeComponent();

		LoadTimeSlots();

		if (!isSetup)
		{
			isSetup = true;
			SetupTrayIcon();
		}
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

		// var secondWindow = new Window
		// {
		// 	Page = new TimerDonePopUpPage { Title = "Timer Done" }
		// };
		// Application.Current.OpenWindow(secondWindow);

		var tm = Shared.Global.MyTimer;
		var selected = allTimeSlots[pickerCurrentTimeSlot.SelectedIndex];
		tm.Init(DateTime.Now, selected, progressBar.ProgressTo, this.PopTimeupPage);
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

	private void SetupTrayIcon()
	{
		var trayService = ServiceProvider.GetService<ITrayService>();

		if (trayService != null)
		{
			trayService.Initialize();
			trayService.ClickHandler = () =>
				ServiceProvider.GetService<INotificationService>()
					?.ShowNotification("Hello Build! 😻 From .NET MAUI", "How's your weather?  It's sunny where we are 🌞");
		}
	}

	private async void PopTimeupPage()
	{
		// await Dispatcher.DispatchAsync(async () =>
		// {
		//      await Navigation.PushModalAsync(new TimerDonePopUpPage(), false);
		// }).ConfigureAwait(false);

		await Dispatcher.DispatchAsync(async () =>
		{
			var secondWindow = new Window
			{
				Page = new TimerDonePopUpPage { Title = "Timer Done" },
				Height = 50,
				Width = 60,
				MaximumHeight = 250,
				MaximumWidth = 350
			};
			Application.Current.OpenWindow(secondWindow);

		}).ConfigureAwait(false);
	}
}

