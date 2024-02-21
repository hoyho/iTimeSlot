using System;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using SkiaSharp;

namespace Foundation
{


    public class Imaging
    {

        public static SKBitmap GenerateTrayIcon(double percentage)
        {
            // 确保百分比在0到100之间
            percentage = Math.Max(0, Math.Min(100, percentage));

            int iconSize = 22; // macOS托盘图标的大小为22x22像素

            using (var surface = SKSurface.Create(new SKImageInfo(iconSize, iconSize)))
            {
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
        }

        public static void ResetTrayIcon()
        {
            try
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                WindowIcon icon = new WindowIcon(AssetLoader.Open(new Uri($"avares://{assemblyName}/Assets/avalonia-logo.ico")));
                TrayIcon.GetIcons(Application.Current).First().Icon = icon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}