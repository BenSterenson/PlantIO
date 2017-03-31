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
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using PlantIO_RestAPI;
using System.Threading.Tasks;
using PlantIO.Modules;
using System.IO;
using System.Json;
using System.Net;
using System.Collections.Generic;

namespace PlantIO
{
	public partial class PlantIOPage : ContentPage
	{
		public IDevice currDevice;
		public IBluetoothLE _bluetooth = CrossBluetoothLE.Current;
		public IAdapter _bleAdapter = CrossBluetoothLE.Current.Adapter;
		public ObservableCollection<IDevice> _bleDevices = new ObservableCollection<IDevice> {};
		public IDevice selectedDevice;
        private bool _keepPolling = false;

        private static int _light;
        private static int _soilMoisture;
        private static int _temp;


        public PlantIOPage()
		{
			InitializeComponent();


            sm_sensor.Text = _soilMoisture.ToString();
            light_sensor.Text = _light.ToString();
            temp_sensor.Text = _temp.ToString();

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
            _bleDevices.Clear();
            _bleAdapter.StartScanningForDevicesAsync();
        }


		async void OnButtonClickedUpdate(object sender, EventArgs e)
		{
			try
			{
				var service = await selectedDevice.GetServiceAsync(Guid.Parse("F0001130-0451-4000-B000-000000000000"));
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

		void OnButtonClickedStart(object sender, EventArgs e)
		{
            _keepPolling = true;
            ContinuousWebRequest();
            return;
		}

        private async void ContinuousWebRequest()
        {
            while (_keepPolling)
            {
                Status.Text = "Status : " + await RequestTimeAsync() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");
                sm_sensor.Text = _soilMoisture.ToString();
                light_sensor.Text = _light.ToString();
                if (_keepPolling)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                }
            }
        }

        static async Task<string> RequestTimeAsync()
        {
            // RestUrl = "http://devices.v1.growos.com/handshake/soilmoisture.0"}
            PlantIOHandshake item = new PlantIOHandshake();

            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(Constants.RestUrl);
                var HR = JsonConvert.DeserializeObject<PlantIOHandshake>(jsonString);

                string url_report = "http://" + HR.host + ":" + HR.port + "/" + HR.path + "/report";

                List<PlantIOData> PlantIOData_Arr = new List<PlantIOData>();

                Random rnd = new Random();
                _soilMoisture = rnd.Next(0, 100);
                _light = rnd.Next(1, 100000);

                PlantIOData sm_obj = new PlantIOData(new PlantIOSrc("soilmoisture.0", "soilmoisture"), "%", _soilMoisture);
                PlantIOData light_obj = new PlantIOData(new PlantIOSrc("light.ambient.0", "light"), "lux", _light);
                PlantIOData temp_obj = new PlantIOData(new PlantIOSrc("temp.0", "temperature"), "c", _temp);

                PlantIOData_Arr.Add(sm_obj);
                PlantIOData_Arr.Add(light_obj);
                PlantIOData_Arr.Add(temp_obj);

                var stringLigh_obj = await Task.Run(() => JsonConvert.SerializeObject(PlantIOData_Arr));

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(stringLigh_obj, Encoding.UTF8, "application/json");

                using (var client_report = new HttpClient())
                {

                    // Do the actual request and await the response
                    var httpResponse = await client_report.PostAsync(url_report, httpContent);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return "OK";
                    }

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        return responseContent;
                        // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                    }

                    return "Failed";
                }
            }

        }

    }

}
