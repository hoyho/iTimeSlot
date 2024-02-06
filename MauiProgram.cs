using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using iTimeSlot.Services;

namespace iTimeSlot;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ITrayService, MacCatalyst.TrayService>();
            builder.Services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();

		return builder.Build();
	}
}
