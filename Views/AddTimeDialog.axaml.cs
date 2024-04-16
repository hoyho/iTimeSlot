using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace iTimeSlot.Views
{
    public partial class AddTimeDialog : Window
    {

        public AddTimeDialog(Object inCtx)
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = inCtx;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
