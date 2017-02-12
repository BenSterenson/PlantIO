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
using System.Linq;

namespace PlantIO
{
	public partial class PlantIOPage : ContentPage
	{
		public IDevice currDevice;
		public IBluetoothLE _bluetooth = CrossBluetoothLE.Current;
		public IAdapter _bleAdapter = CrossBluetoothLE.Current.Adapter;
		public ObservableCollection<IDevice> _bleDevices = new ObservableCollection<IDevice> {};
		public IDevice selectedDevice;


		public PlantIOPage()
		{
			InitializeComponent();

			blePicker.SelectedIndexChanged += (sender, args) =>
			{
				if (blePicker.SelectedIndex == -1)
				{
					devName.Text = "no Device selected";
					selectedDevice = null;
				}
				else
				{
					string bleDevName = blePicker.Items[blePicker.SelectedIndex];
					selectedDevice = _bleDevices[blePicker.SelectedIndex];
					devName.Text = bleDevName;
				}
			};

			_bleAdapter.DeviceDiscovered += bleDiscovered;

		}


		public async void bleDiscovered(object sender, DeviceEventArgs e)
		{
			currDevice = e.Device;
			//Grep the first device only.  
			if (_bleAdapter.IsScanning)
				await _bleAdapter.StopScanningForDevicesAsync();
			
			if (!_bleDevices.Any(dev => dev.Id == currDevice.Id))
			{
				_bleDevices.Add(currDevice);
				this.blePicker.Items.Clear();

				foreach (var dev in _bleDevices)
				{
					this.blePicker.Items.Add(dev.Name);
				}
			}

		}


		public void OnButtonClicked(object sender, EventArgs e)
		{
			_bleAdapter.StartScanningForDevicesAsync();
		}


		async void OnButtonClickedConnect(object sender, EventArgs e)
		{

			try
			{
				await _bleAdapter.ConnectToDeviceAsync(selectedDevice);
				await DisplayAlert("Connected", "Connected", "OK");

			}
			catch (DeviceConnectionException element)
			{
				await DisplayAlert("failed", element.Message, "OK");
			}
		}

	}

}
