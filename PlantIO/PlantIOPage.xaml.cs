using Xamarin.Forms;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Utils;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using PlantIO_RestAPI;
using System.Threading.Tasks;
using PlantIO.Modules;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlantIO.ViewModels;

namespace PlantIO
{
	public partial class PlantIOPage : ContentPage
    {
		public IDevice currDevice;
		public IBluetoothLE _bluetooth = CrossBluetoothLE.Current;
		public IAdapter _bleAdapter = CrossBluetoothLE.Current.Adapter;
		public ObservableCollection<IDevice> _bleDevices = new ObservableCollection<IDevice> {};
		public static IDevice selectedDevice;
        public static int selectedSampleRate;

        public PlantIOPage()
		{
			InitializeComponent();

            this.sampleRatePicker.SelectedIndexChanged += (sender, args) =>
            {
                selectedSampleRate = this.sampleRatePicker.SelectedIndex;
            };

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
			_bleAdapter.StartScanningForDevicesAsync(dev => dev.Name.Contains("Project"));

		}

		public void bleDiscovered(object sender, DeviceEventArgs e)
		{
			currDevice = e.Device;

			if (!_bleDevices.Any(dev => dev.Id == currDevice.Id))
			{
				_bleDevices.Add(currDevice);
				this.blePicker.Items.Clear();

				foreach (var dev in _bleDevices)
				{
					this.blePicker.Items.Add(dev.Name + ": " + dev.Id);
				}
			}

		}

		public void OnButtonClicked(object sender, EventArgs e)
		{
            try
            {
                Disconnect();
            }
            finally
            {
                this.blePicker.Items.Clear();
                selectedDevice = null;
                _bleDevices.Clear();
                _bleAdapter.StartScanningForDevicesAsync(dev => dev.Name.Contains("Project"));
            }
        }

        public async Task Disconnect()
        {
            if (selectedDevice != null)
            {
                await _bleAdapter.DisconnectDeviceAsync(selectedDevice);
                selectedDevice = null;
            }
        }
    }
}
