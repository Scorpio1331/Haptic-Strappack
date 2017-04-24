using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware.Usb;
using Hoho.Android.UsbSerial.Extensions;
using Hoho.Android.UsbSerial.Driver;

namespace Haptic_Strappack_App
{
    [Activity(Label = "Haptic Strappack App Menu", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainMenu : Activity
    {

        public const string EXTRA_TAG = "PortInfo";
        UsbManager usbManager;
        UsbSerialPort port;

        TextView txtvwSelectedDevice;
        Button btnSelectDevice;

        TextView txtvwFolderSelected;
        Button btnSelectLibrary;

        TextView txtvwTitleSongSelection;
        Button btnSelectSong;

        Button btnMediaPlayer;



        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.mainMenu);

            txtvwSelectedDevice = FindViewById<TextView>(Resource.Id.txtvwSelectedDevice);
            btnSelectDevice = FindViewById<Button>(Resource.Id.btnSelectDevice);
            btnSelectDevice.Click += BtnSelectDevice_Click;

            txtvwFolderSelected = FindViewById<TextView>(Resource.Id.txtvwFolderSelected);
            btnSelectLibrary = FindViewById<Button>(Resource.Id.btnSelectLibrary);

            txtvwTitleSongSelection = FindViewById<TextView>(Resource.Id.txtvwTitleSongSelection);
            btnSelectSong = FindViewById<Button>(Resource.Id.btnSelectSong);

            btnMediaPlayer = FindViewById<Button>(Resource.Id.btnMediaPlayer);
            btnMediaPlayer.Click += BtnMediaPlayer_Click;

            usbManager = GetSystemService(UsbService) as UsbManager;

            var portInfo = (UsbSerialPortInfo)Intent.GetParcelableExtra(EXTRA_TAG) ?? null;
            if (portInfo != null)
            {
                int vendorId = portInfo.VendorId;
                int deviceId = portInfo.DeviceId;
                int portNumber = portInfo.PortNumber;
                var drivers = await DeviceSelection.FindAllDriversAsync(usbManager);
                var driver = drivers.Where((d) => d.Device.VendorId == vendorId && d.Device.DeviceId == deviceId).FirstOrDefault();
                if (driver == null)
                    throw new Exception("Driver specified in extra tag not found.");

                port = driver.Ports[portNumber];
                if (port == null)
                {
                    txtvwSelectedDevice.Text = "No serial device selected.";
                    return;
                }

                txtvwSelectedDevice.Text = "Serial device: " + port.Driver.Device.ProductName;
                btnMediaPlayer.Enabled = true;
            }

        }

        private void BtnMediaPlayer_Click(object sender, EventArgs e)
        {
            var newIntent = new Intent(this, typeof(MediaPlayer));
            
            newIntent.PutExtra(MediaPlayer.EXTRA_TAG, new UsbSerialPortInfo(port));
            StartActivity(newIntent);
        }

        private void BtnSelectDevice_Click(object sender, EventArgs e)
        {
            var newIntent = new Intent(this, typeof(DeviceSelection));
            StartActivity(newIntent);

        }
    }
}