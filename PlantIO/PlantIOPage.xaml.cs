using Xamarin.Forms;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Utils;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;

namespace PlantIO
{
	public partial class PlantIOPage : ContentPage
	{
		public IDevice device;

		public PlantIOPage()
		{
			InitializeComponent();

			blePicker.SelectedIndexChanged += (sender, args) =>
			{
				if (blePicker.SelectedIndex == -1)
				{
					devName.Text = "no Device selected";
					device = null;
				}
				else
				{
					string bleDevName = blePicker.Items[blePicker.SelectedIndex];
					devName.Text = bleDevName;
				}
			};

		}

		//IBluetoothLE ble = CrossBluetoothLE.Current;
		IAdapter adapter = CrossBluetoothLE.Current.Adapter;

		//ble.StateChanged += (s, c) => {Debug.WriteLine($"The bluetooth state changed to {c.NewState}");};

		//adapter.DeviceDiscovered += (s, a) =>{Debug.WriteLine($"Device found: {a.Device.Name}");};

		//adapter.StartScanningForDevicesAsync();

		public void OnButtonClicked(object sender, EventArgs e)
		{
			adapter.StartScanningForDevicesAsync();
			//var mylist = adapter.DiscoveredDevices;
			//var device = adapter.DiscoverDeviceAsync(dev => dev.Name.Equals("Project Zero"));
			//devName.Text = device.ToString();

			var ListViewItems = new ObservableCollection<string> { "one", "two", "three" };

			this.blePicker.Items.Clear();

			foreach (var num in ListViewItems)
			{
				blePicker.Items.Add(num);
			}
		}

		public void OnButtonClickedConnect(object sender, EventArgs e)
		{
			adapter.ConnectToDeviceAsync(device);

		}

	}
}
