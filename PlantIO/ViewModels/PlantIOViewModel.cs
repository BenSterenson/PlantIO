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
using Xamarin.Forms;


namespace PlantIO.ViewModels
{
    public class PlantIOViewModel : MvxViewModel, INotifyPropertyChanged
    {
        /************************/
        /*VARIABLE DECELERATIONS*/
        /************************/
        
        public new event PropertyChangedEventHandler PropertyChanged;
        public ICharacteristic Characteristic { get; private set; }
        public string CharacteristicValue => Characteristic?.Value.ToHexString().Replace("-", " ");
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        private string status;
        private string api_url;

        private static int sm_sensor;
        private static int light_sensor;
        private static int temp_sensor;
        private static int id_sample;
        private string updateButtonText;
        private bool _updatesStarted;

        private bool _keepPolling;
        /*****************************/
        /*END VARIABLE DECELERATIONS*/
        /****************************/

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
                Random rnd = new Random();
                Light_sensor = rnd.Next(1, 100000);
                Temp_sensor = rnd.Next(0, 30);
                //ContinuousWebRequest();
                OnPropertyChanged();
            }
        }
        public string Api_url
        {
            get
            {
                return api_url;
            }

            set
            {
                api_url = value;
                OnPropertyChanged();
            }
        }

        public PlantIOViewModel()
        {
            sm_sensor = 0;
            light_sensor = 0;
            temp_sensor = 0;
            id_sample = 1;
            status = "0";
            ChangeButtonStart(false);
            api_url = Constants.PERFIX_REST_URL;
        }

        private void ChangeButtonStart(bool value)
        {
            _updatesStarted = value;

            if (_updatesStarted == true)
            {
                UpdateButtonText = "Stop updates";
                return;
            }
            UpdateButtonText = "Start updates";
        }

        /****************/
        /*BUTTON COMMAND*/
        /****************/

        /*Read*/
        public MvxCommand OnButtonClickedStart => new MvxCommand((() =>
        {

            _keepPolling = true;
            ContinuousWebRequest();
        }));
        private async void ReadSoilMoisture()
        {
            var selectedDevice = PlantIOPage.selectedDevice;
            if (selectedDevice == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "BLE device not found. Try reconnecting to device", "OK");
                return;
            }

            try
            {
                ChangeButtonStart(true);

                var service = await selectedDevice.GetServiceAsync(Guid.Parse("0000BA55-0000-1000-8000-00805F9B34FB"));
                Characteristic = await service.GetCharacteristicAsync(Guid.Parse("00002BAD-0000-1000-8000-00805F9B34FB"));
                var data = Characteristic.Value;

                SM_sensor = BitConverter.ToInt32(data, 0);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed sensor registration. Info: " + ex.ToString(), "OK");
                return;
            }
        }

        /*Notify*/
        public MvxCommand ToggleUpdatesCommand => new MvxCommand((() =>
            {
                if (_updatesStarted)
                    StopUpdates();

                else
                    StartUpdates();
            }));
        private async void StartUpdates()
        {
            var selectedDevice = PlantIOPage.selectedDevice;
            if (selectedDevice == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "BLE device not found. Try reconnecting to device", "OK");
                return;
            }
            if (Api_url == Constants.PERFIX_REST_URL)
                Api_url = Constants.DEFAULT_REST_URL;

            try
            {
                ChangeButtonStart(true);

                var service = await selectedDevice.GetServiceAsync(Guid.Parse(Constants.SERVICE_STR));
                Characteristic = await service.GetCharacteristicAsync(Guid.Parse(Constants.CHARACTERISRIC_STR));

                Characteristic.ValueUpdated -= CharacteristicOnValueUpdated;
                Characteristic.ValueUpdated += CharacteristicOnValueUpdated;
                await Characteristic.StartUpdatesAsync();

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed sensor registration. Info: " + ex.ToString(), "OK");
                return;
            }
        }
        private async void StopUpdates()
        {
            try
            {
                ChangeButtonStart(false);

                await Characteristic.StopUpdatesAsync();
                Characteristic.ValueUpdated -= CharacteristicOnValueUpdated;

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed sensor registration. Info: " + ex.ToString(), "OK");
                return;
            }
        }
        /********************/
        /*END BUTTON COMMAND*/
        /********************/

        /********************/
        /*REST API FUNCTIONS*/
        /********************/
        private async void ContinuousWebRequest()
        {
            while (_keepPolling)
            {
                Status = await ReportAsyncGoogle() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");
                if (_keepPolling)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                }
            }
        }
        private async Task<string> ReportAsyncGoogle()
        {
            // RestUrl = "https://sheetsu.com/apis/v1.0/5384ce7c1dc4"}
            List<PlantIOSample> PlantIOData_Arr = new List<PlantIOSample>();

            ReadSoilMoisture();


            var timeStamp = DateTime.Now.ToString("dd/MM/yy H:mm:ss");

            PlantIOSample sm_obj = new PlantIOSample(id_sample, timeStamp, "soilmoisture", "%", sm_sensor);
            PlantIOSample light_obj = new PlantIOSample(id_sample, timeStamp, "light", "lux", light_sensor);
            PlantIOSample temp_obj = new PlantIOSample(id_sample, timeStamp, "temperature", "c", temp_sensor);

            PlantIOData_Arr.Add(sm_obj);
            PlantIOData_Arr.Add(light_obj);
            PlantIOData_Arr.Add(temp_obj);

            PlantIORestApi RestApi_post_obj = new PlantIORestApi(PlantIOData_Arr);
            var json_obj_str = await Task.Run(() => JsonConvert.SerializeObject(RestApi_post_obj));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(json_obj_str, Encoding.UTF8, "application/json");

            using (var client_report = new HttpClient())
            {

                // Do the actual request and await the response
                var httpResponse = await client_report.PostAsync(Api_url, httpContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    id_sample++;
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
        /************************/
        /*END REST API FUNCTIONS*/
        /************************/

        /***************************/
        /*INotifyProperty FUNCTIONS*/
        /***************************/
        private void CharacteristicOnValueUpdated(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
            Messages.Insert(0, $"Updated value: {CharacteristicValue}");
            SM_sensor = int.Parse(CharacteristicValue, System.Globalization.NumberStyles.HexNumber);
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /*******************************/
        /*END INotifyProperty FUNCTIONS*/
        /*******************************/
    }
}
