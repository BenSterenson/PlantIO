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

        enum e_sampleRate { min, hours, days };
        private bool _updatesStarted;
        private bool _readPeriodicStarted;
        private bool isValid;
        
        private string status;
        private string api_url;
        private string readPeriodicButtonText;
        private string updateButtonText;
        private static int sm_sensor;
        private static int light_sensor;
        private static int temp_sensor;
        private static int id_sample;
        private int sampleRate;
        private string ble_sample_type;

        /*****************************/
        /*END VARIABLE DECELERATIONS*/
        /****************************/

        /*******************/
        /*BINDING FUNCTIONS*/
        /*******************/
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
        public string ReadPeriodicButtonText
        {
            get
            {
                return readPeriodicButtonText;
            }

            set
            {
                readPeriodicButtonText = value;
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
                /* Send new values to RestAPI */
                SendToRestApi();
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
        public int SampleRate
        {
            get
            {
                return sampleRate;
            }

            set
            {
                sampleRate = value;
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
            sm_sensor = 0;
            light_sensor = 0;
            temp_sensor = 0;
            id_sample = 1;
            sampleRate = 5;
            status = "0";

            ChangeReadPeriodicButton(false);
            ChangeButtonStart(false);
            api_url = Constants.PERFIX_REST_URL;
        }
        /********************/
        /*END VM CONSTRUCTOR*/
        /********************/

        /****************/
        /*BUTTON COMMAND*/
        /****************/
        /*Read*/
        private void ChangeReadPeriodicButton(bool value)
        {
            _readPeriodicStarted = value;

            if (_readPeriodicStarted == true)
            {
                ReadPeriodicButtonText = "Stop periodic read";
                IsValid = false;
                return;
            }
            if(_updatesStarted == false)
                IsValid = true;
            ReadPeriodicButtonText = "Start periodic read";
        }
        public MvxCommand OnButtonClickedReadPeriodic => new MvxCommand((() =>
        {
            if (_readPeriodicStarted)
                ChangeReadPeriodicButton(false);

            else
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
            if (Api_url == Constants.PERFIX_REST_URL)
                Api_url = Constants.DEFAULT_REST_URL;
            try
            {
                ChangeReadPeriodicButton(true);

                var service = await selectedDevice.GetServiceAsync(Guid.Parse(Constants.SERVICE_STR));
                Characteristic = await service.GetCharacteristicAsync(Guid.Parse(Constants.CHARACTERISRIC_STR));
                var data = await Characteristic.ReadAsync();
                
                if(data.Length > 0)
                {
                    var temp = BitConverter.ToString(data);
                    var arr = temp.Split('-');
                    try
                    {
                        Temp_sensor = int.Parse(arr[0], System.Globalization.NumberStyles.HexNumber);
                        SM_sensor = int.Parse(arr[1], System.Globalization.NumberStyles.HexNumber);
                    }
                    catch (Exception ex)
                    {
                        Status = "Error parsing : " + CharacteristicValue;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed sensor registration. Info: " + ex.ToString(), "OK");
                return;
            }
        }

        /*Notify*/
        private void ChangeButtonStart(bool value)
        {
            _updatesStarted = value;

            if (_updatesStarted == true)
            {
                UpdateButtonText = "Stop updates";
                IsValid = false;
                return;
            }
            if (_readPeriodicStarted == false)
                IsValid = true;
            UpdateButtonText = "Start updates";
        }
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
        
        private int ConvertToSec(int selectedSampleRate)
        {
            switch (selectedSampleRate) 
            {
                case (int)e_sampleRate.min:
                    return 60;
                case (int)e_sampleRate.hours:
                    return 3600;
                case (int)e_sampleRate.days:
                    return 86400;
                default:
                    return 1;
            }
        }
        private async void SendToRestApi()
        {
            Status = await ReportAsyncGoogle() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");
        }
        private async void ContinuousWebRequest()
        {
            ChangeReadPeriodicButton(true);
            while (_readPeriodicStarted)
            {
                ble_sample_type = "Read Periodic";
                ReadSoilMoisture();
                if (_readPeriodicStarted)
                {
                    await Task.Delay(TimeSpan.FromSeconds(ConvertToSec(PlantIOPage.selectedSampleRate) * sampleRate));
                }
            }
        }
        private async Task<string> ReportAsyncGoogle()
        {
            // RestUrl = "https://sheetsu.com/apis/v1.0/5384ce7c1dc4"}
            List<PlantIOSample> PlantIOData_Arr = new List<PlantIOSample>();

            var date = DateTime.Now.ToString("dd-MM-yyyy");
            var time = DateTime.Now.ToString("HH:mm:ss");
            var sample_type = ble_sample_type;

            PlantIOSample sm_obj = new PlantIOSample(id_sample, date, time, "soilmoisture", "%", sm_sensor, sample_type);
            PlantIOSample light_obj = new PlantIOSample(id_sample, date, time, "light", "lux", light_sensor, sample_type);
            PlantIOSample temp_obj = new PlantIOSample(id_sample, date, time, "temperature", "c", temp_sensor, sample_type);

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
            ble_sample_type = "Notify";
            var arr = CharacteristicValue.Split(' ');
            try
            {
                Temp_sensor = int.Parse(arr[0], System.Globalization.NumberStyles.HexNumber);
                SM_sensor = int.Parse(arr[1], System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception ex)
            {
                Status = "Error parsing : "+ CharacteristicValue;
                return;
            }

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
