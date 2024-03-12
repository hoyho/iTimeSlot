using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            // 确保百分比在0到100之间
            percentage = Math.Max(0, Math.Min(100, percentage));

            int iconSize = 22; // macOS托盘图标的大小为22x22像素

            using var surface = SKSurface.Create(new SKImageInfo(iconSize, iconSize));
            SKCanvas canvas = surface.Canvas;

            // 清空画布
            canvas.Clear(SKColors.Transparent);

            // 绘制饼状图
            SKPaint paint = new SKPaint
            {
                IsAntialias = true
            };

            SKRect rect = new SKRect(0, 0, iconSize, iconSize);
            SKColor fillColor = SKColors.White; // 饼状图的填充颜色，可以根据需要调整
            paint.Color = fillColor;
            canvas.DrawOval(rect, paint);

            // 绘制扇形
            paint.Color = SKColors.MediumSlateBlue; // 扇形的颜色，可以根据需要调整

            float startAngle = -90; // 从12点钟方向开始
            float sweepAngle = (float)(360 * (1 - percentage / 100));
            canvas.DrawArc(rect, startAngle, sweepAngle, true, paint);

            // 生成位图对象
            SKImage image = surface.Snapshot();
            return SKBitmap.FromImage(image);
        }

        public void SetPercentageTrayIcon(double percentage)
        {
            using var stream = new MemoryStream();
            using var skBitmap = GenerateTrayIcon(percentage);
            skBitmap.Encode(stream, SKEncodedImageFormat.Png, 100); // 将 SKBitmap 对象保存为 PNG 格式到流中
            // 将流的位置重置到起始位置，以便后续读取
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