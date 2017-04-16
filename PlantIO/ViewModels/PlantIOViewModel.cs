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
        private bool _updatesStarted;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICharacteristic Characteristic { get; private set; }
        public string CharacteristicValue => Characteristic?.Value.ToHexString().Replace("-", " ");
        
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        Page _page;

        private int readValue;
        private string status;
        private static int sm_sensor;
        private static int light_sensor;
        private static int temp_sensor;
        private string updateButtonText;


        private bool _keepPolling;

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
                OnPropertyChanged();
                }
            }


            public PlantIOViewModel()
            {
                readValue = 0;
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
                    UpdateButtonText = "Stop updates";
                    return;
                }
                UpdateButtonText = "Start updates";
            }
        public MvxCommand ToggleUpdatesCommand => new MvxCommand((() =>
            {
                if (_updatesStarted)
                {
                    StopUpdates();
                    _keepPolling = false;
                }
                else
                {
                    StartUpdates();
                    _keepPolling = true;
                    //ContinuousWebRequest();
                }
            }));

            public MvxCommand OnButtonClickedStart => new MvxCommand((() =>
            {
                Random rnd = new Random();
                /*SM_sensor = rnd.Next(0, 100);
                /Light_sensor = rnd.Next(1, 100000);
                Temp_sensor = rnd.Next(0, 30);
                */
                _keepPolling = true;
            //ContinuousWebRequest();
        }));

        

        private async void StartUpdates()
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


                Characteristic.ValueUpdated -= CharacteristicOnValueUpdated;
                Characteristic.ValueUpdated += CharacteristicOnValueUpdated;
                await Characteristic.StartUpdatesAsync();


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
                //Status = await RequestTimeAsync() + ", Sent Time: " + DateTime.Now.ToString("HH:mm:ss");
                if (_keepPolling)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                }
            }
        }

        



        private void CharacteristicOnValueUpdated(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {
            Messages.Insert(0, $"Updated value: {CharacteristicValue}");
            
            SM_sensor = int.Parse(CharacteristicValue, System.Globalization.NumberStyles.HexNumber);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
