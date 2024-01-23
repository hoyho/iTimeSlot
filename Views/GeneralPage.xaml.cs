namespace iTimeSlot.Views;

public partial class GeneralPage : ContentPage
{
	List<string> _remind_befores = new List<string> { "1 minute before", "2 minute before", "5 minute before", "10 minute before" }; 
	int count = 0;

	public GeneralPage()
	{
		InitializeComponent();
		
		// set the default value for the picker because the bug in macOS that not binding the value
		//see also https://github.com/dotnet/maui/issues/10208
		picker.SelectedIndex = 1;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
	}
}

