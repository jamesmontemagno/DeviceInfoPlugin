using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Plugin.DeviceInfo;

namespace DeviceInfoSample.Desktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			_ = new Window()
			{
				Content = new StackPanel()
				{
					Margin = new Thickness(50),
					VerticalAlignment = VerticalAlignment.Center,
					Children =
					{
						new TextBlock { Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId() },
						new TextBlock { Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true) },
						new TextBlock { Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello") },
						new TextBlock
						{
							Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello", "world")
						},
						new TextBlock { Text = "Id: " + CrossDeviceInfo.Current.Id },
						new TextBlock { Text = "Model: " + CrossDeviceInfo.Current.Model },
						new TextBlock { Text = "Platform: " + CrossDeviceInfo.Current.Platform },
						new TextBlock { Text = "Version: " + CrossDeviceInfo.Current.Version },
					}
				}
			}.ShowDialog();
		}
	}
}
