namespace iTimeSlot.Views;


public partial class GeneralPage : ContentPage
{
	List<string> _remind_befores = iTimeSlot.Shared.DefaultConfig.RemindBefores.ToList();

	public GeneralPage()
	{
		InitializeComponent();

		// set the default value for the picker because the bug in macOS that not binding the value
		//see also https://github.com/dotnet/maui/issues/10208
		picker.SelectedIndex = 1;
	}

	public void LoadUserDefinedSettings()
	{
		//todo
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
	}
}

