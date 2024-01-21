using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace iTimeSlot.Views
{
    public static class Common
    {
        public static async Task ShowToastAsync(string text)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}