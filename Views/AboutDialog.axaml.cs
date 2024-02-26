using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace iTimeSlot.Views
{
    public partial class AboutDialog : Window
    {
        private static readonly Version s_version = typeof(AboutDialog).Assembly.GetName().Version;

        public static string Version { get; } = s_version.ToString(2);

        public static bool IsDevelopmentBuild { get; } = s_version.Revision == 999;

        public AboutDialog()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
