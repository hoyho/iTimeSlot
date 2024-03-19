
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using MsBox.Avalonia.ViewModels;
using MsBox.Avalonia.Windows;

namespace iTimeSlot.Foundation
{
    internal class AlertBox
    {
        readonly MsBoxWindow window;
        readonly MsBoxCustomView msBoxCustomView;
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

        public AlertBox()
        {
            var msBoxCustomViewModel = new MsBoxCustomViewModel(new MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>
                     {
                        new ButtonDefinition { Name = "Ok", },
                        new ButtonDefinition { Name = "Restart", },
                     },
                ContentTitle = "Timer done",
                ContentMessage = "Have a break",
                Icon = MsBox.Avalonia.Enums.Icon.Info,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                MaxWidth = 600,
                MaxHeight = 900,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = false,
            });

            this.msBoxCustomView = new MsBoxCustomView
            {
                DataContext = msBoxCustomViewModel
            };
            msBoxCustomViewModel.SetFullApi(msBoxCustomView);

            this.window = new MsBoxWindow
            {
                Content = msBoxCustomView,
                DataContext = msBoxCustomViewModel
            };
            window.Closed += msBoxCustomView.CloseWindow;

            msBoxCustomView.SetCloseAction(() =>
            {
                tcs.TrySetResult(msBoxCustomView.GetButtonResult());
                window.Close();
            });

        }


        public Task<string> ShowAlertAsync()
        {
            window.Show();
            return tcs.Task;
        }

        public void Close()
        {
            msBoxCustomView.CloseWindow(null, null);
            window.Close();
        }

    }

}
