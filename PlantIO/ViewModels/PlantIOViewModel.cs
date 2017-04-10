using System;
using System.Text;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PlantIO.Modules;
using PlantIO_RestAPI;

namespace PlantIO.ViewModels
{
    public class PlantIOViewModel : MvxViewModel, INotifyPropertyChanged
    {
        /************************/
        /*VARIABLE DECELERATIONS*/
        /************************/
        public event PropertyChangedEventHandler PropertyChanged;
        public ICharacteristic Characteristic { get; private set; }
        public string CharacteristicValue => Characteristic?.Value.ToHexString().Replace("-", " ");
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        private bool _updatesStarted;
        //private bool _keepPolling;

        private int readValue;
        private string status;
        private string api_url;
        private string updateButtonText;
        private static int sm_sensor;
        private static int light_sensor;
        private static int temp_sensor;
        private static int id_sample;
        private bool isValid;
        /*****************************/
        /*END VARIABLE DECELERATIONS*/
        /****************************/

        /*******************/
        /*BINDING FUNCTIONS*/
        /*******************/
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
        public int ReadValue
        {
            get
            {
                return readValue;
            }

            set
            {
                readValue = value;
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
        public bool IsValid
        {
            get
            {
                return isValid;
            }

            set
            {
                isValid = value;
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
        /***********************/
        /*END BINDING FUNCTIONS*/
        /***********************/

        /****************/
        /*VM CONSTRUCTOR*/
        /****************/
        public PlantIOViewModel()
        {
            readValue = 0;
            sm_sensor = 0;
            light_sensor = 0;
            temp_sensor = 0;
            id_sample = 1;
            status = "0";
            ChangeButtonLable(false);
            api_url = Constants.PERFIX_REST_URL;
        }
        /********************/
        /*END VM CONSTRUCTOR*/
        /********************/

        /****************/
        /*BUTTON COMMAND*/
        /****************/
        public MvxCommand ToggleUpdatesCommand => new MvxCommand((() =>
        {
            if (_updatesStarted)
            {
                //_keepPolling = false;
                StopUpdates();
            }
            else
            {
                //_keepPolling = true;

                StartUpdates();
                ContinuousWebRequest();
            }
        }));
        private void ChangeButtonLable(bool value)
        {
            _updatesStarted = value;

            if (_updatesStarted == true)
            {
                UpdateButtonText = "Stop updates";
                IsValid = false;
                return;
            }
            IsValid = true;
            UpdateButtonText = "Start updates";
        }
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
                ChangeButtonLable(true);
                var service = await selectedDevice.GetServiceAsync(Guid.Parse(Constants.SERVICE_STR));
                Characteristic = await service.GetCharacteristicAsync(Guid.Parse(Constants.CHARACTERISRIC_STR));

                Characteristic.ValueUpdated -= CharacteristicOnValueUpdated;
                Characteristic.ValueUpdated += CharacteristicOnValueUpdated;
                await Characteristic.StartUpdatesAsync();
                

            }
            catch (Exception ex)
            {
                //_keepPolling = false;
                await App.Current.MainPage.DisplayAlert("Error", "Failed sensor registration. Info: " + ex.ToString(), "OK");
                return;
            }
        }
        private async void StopUpdates()
        {
            try
            {
                ChangeButtonLable(false);
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
            while (_updatesStarted)
            {
                //Status = await RequestTimeAsync() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");
                Status = await ReportAsyncGoogle() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");

                if (_updatesStarted)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                }
            }
        }

        private async Task<string> ReportAsyncGoogle()
        {
            // RestUrl = "https://sheetsu.com/apis/v1.0/5384ce7c1dc4"}
            List<PlantIOSample> PlantIOData_Arr = new List<PlantIOSample>();

            Random rnd = new Random();
            SM_sensor = rnd.Next(0, 100);
            Light_sensor = rnd.Next(1, 100000);
            Temp_sensor = rnd.Next(0, 30);
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

        /*private static async Task<string> RequestTimeAsync()
        {
            // RestUrl = "http://devices.v1.growos.com/handshake/soilmoisture.0"}

            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(Constants.REST_URL);
                var HR = JsonConvert.DeserializeObject<PlantIOHandshake>(jsonString);

                return await ReportAsync(HR);
            }
        }
        private static async Task<string> ReportAsync(PlantIOHandshake HR)
        {
            string url_report = "http://" + HR.host + ":" + HR.port + "/" + HR.path + "/report";

            List<PlantIOData_old> PlantIOData_Arr = new List<PlantIOData_old>();

            Random rnd = new Random();
            sm_sensor = rnd.Next(0, 100);
            light_sensor = rnd.Next(1, 100000);
            temp_sensor = rnd.Next(0, 30);
            PlantIOData_old sm_obj = new PlantIOData_old(new PlantIOSrc("soilmoisture.0", "soilmoisture"), "%", sm_sensor);
            PlantIOData_old light_obj = new PlantIOData_old(new PlantIOSrc("light.ambient.0", "light"), "lux", light_sensor);
            PlantIOData_old temp_obj = new PlantIOData_old(new PlantIOSrc("temp.0", "temperature"), "c", temp_sensor);

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
        }*/
        /************************/
        /*END REST API FUNCTIONS*/
        /************************/

        /***************************/
        /*INotifyProperty FUNCTIONS*/
        /***************************/
        private void CharacteristicOnValueUpdated(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
            Messages.Insert(0, $"Updated value: {CharacteristicValue}");
            ReadValue = int.Parse(CharacteristicValue, System.Globalization.NumberStyles.HexNumber);
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
