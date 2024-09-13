using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using SkiaSharp;

namespace iTimeSlot.Foundation
{


    public class TrayHelper
    {

        public SKBitmap GenerateTrayIcon(double percentage)
        {
            percentage = Math.Max(0, Math.Min(100, percentage));

            int iconSize = 22; // 22x22 pixels for macOS tray icon

            using var surface = SKSurface.Create(new SKImageInfo(iconSize, iconSize));
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Transparent);
            SKPaint paint = new SKPaint
            {
                IsAntialias = true
            };

            SKRect rect = new SKRect(0, 0, iconSize, iconSize);
            SKColor fillColor = SKColors.White; // color of the circle
            paint.Color = fillColor;
            canvas.DrawOval(rect, paint);

            // Draw the arc (the percentage part)
            paint.Color = SKColors.MediumSlateBlue; // color of the arc

            float startAngle = -90; // start from the top
            float sweepAngle = (float)(360 * (1 - percentage / 100));
            canvas.DrawArc(rect, startAngle, sweepAngle, true, paint);

            SKImage image = surface.Snapshot();
            return SKBitmap.FromImage(image);
        }

        public void SetPercentageTrayIcon(double percentage)
        {
            using var stream = new MemoryStream();
            using var skBitmap = GenerateTrayIcon(percentage);
            skBitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
            stream.Position = 0;

            var icon = new WindowIcon(stream);
            TrayIcon.GetIcons(Application.Current).First().Icon = icon;
        }

        public void ResetTrayIcon()
        {
            try
            {
                // var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                // WindowIcon wIcon = new WindowIcon(AssetLoader.Open(new Uri($"avares://{assemblyName}/Assets/avalonia-logo.ico")));

                // var icon = new TrayIcon();
                // icon.SetValue(TrayIcon.IconProperty, wIcon);
    

                // TrayIcons icons = [icon];
                // TrayIcon.SetIcons(Application.Current, icons);
                var assemblyName = "iTimeSlot"; // Assembly.GetExecutingAssembly().GetName().Name;
                using var stream = AssetLoader.Open(new Uri($"avares://{assemblyName}/Assets/tray-icon.png"));
                WindowIcon icon = new WindowIcon(stream);
                TrayIcon.GetIcons(Application.Current).First().Icon = icon;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}