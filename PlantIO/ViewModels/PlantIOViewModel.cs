using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using PlantIO.Modules;
using System.Net.Http;
using Newtonsoft.Json;
using PlantIO_RestAPI;
using System.Collections.ObjectModel;
using FreshMvvm.CRM.Models;


namespace PlantIO.ViewModels
{
    public class PlantIOViewModel : MvxViewModel, INotifyPropertyChanged
    {
        private bool _updatesStarted;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICharacteristic Characteristic { get; private set; }
        public string CharacteristicValue => Characteristic?.Value.ToHexString().Replace("-", " ");
        
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        private string currentStatus;
        private string status;
        private static int sm_sensor;
        private static int light_sensor;
        private static int temp_sensor;
        private string updateButtonText;


        private bool _keepPolling;

        public string CurrentStatus
        {
            get
            {
                return currentStatus;
            }

            set
            {
                currentStatus = value;
                OnPropertyChanged();
            }
        }

        public string UpdateButtonText
        {
            get
            {
                return updateButtonText;
            }

            set
            {
                updateButtonText = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged();
            }
        }

        public int Temp_sensor
        {
            get
            {
                return temp_sensor;
            }

            set
            {
                temp_sensor = value;
                OnPropertyChanged();
            }
        }
        public int Light_sensor
        {
            get
            {
                return light_sensor;
            }

            set
            {
                light_sensor = value;
                OnPropertyChanged();
            }
        }

        public int SM_sensor
        {
            get
            {
                return sm_sensor;
            }

            set
            {
                sm_sensor = value;
                OnPropertyChanged();
            }
        }


        public PlantIOViewModel()
        {
            currentStatus = "0";
            sm_sensor = 0;
            light_sensor = 0;
            temp_sensor = 0;
            status = "0";
            ChangeButtonStart(false);

        }

        private void ChangeButtonStart(bool value)
        {
            _updatesStarted = value;

            if (_updatesStarted == true)
            {
                updateButtonText = "Stop updates";
                return;
            }
            updateButtonText = "Start updates";
        }
    public MvxCommand ToggleUpdatesCommand => new MvxCommand((() =>
        {
            if (_updatesStarted)
            {
                StopUpdates();
            }
            else
            {
                StartUpdates();
            }
        }));

        public MvxCommand OnButtonClickedStart => new MvxCommand((() =>
        {
            Random rnd = new Random();
            SM_sensor = rnd.Next(0, 100);
            Light_sensor = rnd.Next(1, 100000);
            Temp_sensor = rnd.Next(0, 30);

            _keepPolling = true;
            //ContinuousWebRequest();
        }));

        

        private async void StartUpdates()
        {
            var selectedDevice = PlantIOPage.selectedDevice;
            if (selectedDevice == null)
            {
                //var answer = await App.Current.MainPage.DisplayAlert("Error", "You are not connected to a BLE device try again",
                //CoreMethods.DisplayAlert("Goodbye World", "", "Ok");

                var answer = await App.Current.MainPage.DisplayAlert("You are not connected to a BLE device try again", "You are not connected to a BLE device try again", "Yes", "No");
                return;
            }

            try
            {
                ChangeButtonStart(true);

                var service = await selectedDevice.GetServiceAsync(Guid.Parse("0000BA55-0000-1000-8000-00805F9B34FB"));
                Characteristic = await service.GetCharacteristicAsync(Guid.Parse("00002BAD-0000-1000-8000-00805F9B34FB"));


                Characteristic.ValueUpdated -= CharacteristicOnValueUpdated;
                Characteristic.ValueUpdated += CharacteristicOnValueUpdated;
                await Characteristic.StartUpdatesAsync();

                Messages.Insert(0, $"Start updates");

                RaisePropertyChanged(() => UpdateButtonText);

            }
            catch (Exception ex)
            {

                var x = 0;

            }
        }


        private async void StopUpdates()
        {
            try
            {
                ChangeButtonStart(false);

                await Characteristic.StopUpdatesAsync();
                Characteristic.ValueUpdated -= CharacteristicOnValueUpdated;

                Messages.Insert(0, $"Stop updates");

                RaisePropertyChanged(() => UpdateButtonText);

            }
            catch (Exception ex)
            {
                var x = 0;
            }
        }


        private async void ContinuousWebRequest()
        {
            while (_keepPolling)
            {
                Status = await RequestTimeAsync() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");
                if (_keepPolling)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                }
            }
        }

        private static async Task<string> RequestTimeAsync()
        {
            // RestUrl = "http://devices.v1.growos.com/handshake/soilmoisture.0"}

            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(Constants.RestUrl);
                var HR = JsonConvert.DeserializeObject<PlantIOHandshake>(jsonString);

                return await ReportAsync(HR);
            }
        }


        private static async Task<string> ReportAsync(PlantIOHandshake HR)
        {
            string url_report = "http://" + HR.host + ":" + HR.port + "/" + HR.path + "/report";

            List<PlantIOData> PlantIOData_Arr = new List<PlantIOData>();

            Random rnd = new Random();
            sm_sensor = rnd.Next(0, 100);
            light_sensor = rnd.Next(1, 100000);
            temp_sensor = rnd.Next(0, 30);
            PlantIOData sm_obj = new PlantIOData(new PlantIOSrc("soilmoisture.0", "soilmoisture"), "%", sm_sensor);
            PlantIOData light_obj = new PlantIOData(new PlantIOSrc("light.ambient.0", "light"), "lux", light_sensor);
            PlantIOData temp_obj = new PlantIOData(new PlantIOSrc("temp.0", "temperature"), "c", temp_sensor);

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



        private void CharacteristicOnValueUpdated(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
            Messages.Insert(0, $"Updated value: {CharacteristicValue}");
            RaisePropertyChanged(() => CharacteristicValue);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
