
using CoreMedia;

namespace iTimeSlot.Views;

public partial class TimerDonePopUpPage : ContentPage
{
	public TimerDonePopUpPage()
	{
		InitializeComponent();
	}
	
	private void OnOkClicked(object sender, EventArgs e)
	{
		Console.WriteLine("OnOkClicked");
		//await Navigation.PopModalAsync();

		// Close the active window
		Application.Current.CloseWindow(GetParentWindow());
	}

	private void OnRestartClicked(object sender, EventArgs e)
	{
	}

}