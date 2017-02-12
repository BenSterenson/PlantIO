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


			blePicker.SelectedIndexChanged += async(sender, args) =>
			{
				if (blePicker.SelectedIndex == -1)
				{
					devName.Text = "No Device selected";
					selectedDevice = null;
				}
				else
				{
					string bleDevName = blePicker.Items[blePicker.SelectedIndex];
					selectedDevice = _bleDevices[blePicker.SelectedIndex];
					await _bleAdapter.StopScanningForDevicesAsync();

					try
					{
						await _bleAdapter.ConnectToDeviceAsync(selectedDevice);
						await DisplayAlert("Connected", "Connected", "OK");
						devName.Text = "Device Name: " + selectedDevice.Name;
						devState.Text = "Device state: " + selectedDevice.State.ToString();

					}
					catch (DeviceConnectionException element)
					{
						await DisplayAlert("failed", element.Message, "OK");
					}
				}
			};

			_bleAdapter.DeviceDiscovered += bleDiscovered;
			_bleAdapter.StartScanningForDevicesAsync();

		}

		public void bleDiscovered(object sender, DeviceEventArgs e)
		{
			currDevice = e.Device;
			//Grep the first device only.  
			//if (_bleAdapter.IsScanning)
			//	await _bleAdapter.StopScanningForDevicesAsync();
			
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


		async void OnButtonClickedUpdate(object sender, EventArgs e)
		{
			var service = await selectedDevice.GetServiceAsync(Guid.Parse("F0001130-0451-4000-B000-000000000000"));
			try
			{
				var characteristic = await service.GetCharacteristicAsync(Guid.Parse("F0001131-0451-4000-B000-000000000000"));

				int rate = Convert.ToInt32(sampleRate.Text);
				byte[] intBytes = BitConverter.GetBytes(rate);

				if (BitConverter.IsLittleEndian)
					Array.Reverse(intBytes);
				byte[] result = intBytes;

				await characteristic.WriteAsync(result);
			}

			catch (Exception element)
			{
				await DisplayAlert("failed", element.Message, "OK");
			}
		}

	}

}
