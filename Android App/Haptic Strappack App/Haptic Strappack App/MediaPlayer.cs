using Android.App;
using Android.Widget;
using Android.OS;
using System.Linq;
using System;
using ManagedBass;
using ManagedBass.Fx;
using Android.Views;
using Hoho.Android.UsbSerial.Driver;
using Hoho.Android.UsbSerial.Extensions;
using Android.Hardware.Usb;
using Android.Content;
using System.Text;

namespace Haptic_Strappack_App
{
    [Activity(Label = "Media Player")]
    public class MediaPlayer : Activity
    {
        public const string EXTRA_TAG = "PortInfo";
        UsbManager usbManager;
        UsbSerialPort port;
        SerialInputOutputManager serialIoManager;

        private TextView txtvwBPM;
        private TextView txtvwVol;
        private TextView txtvwPorts;
        private TextView txtvwError;
        private TextView txtvwSongName;
        private TextView txtvwTime;
        private ProgressBar progbarTime;

        private double time = 0;
        private const float bpmTimePeriod = 10f;
        private int stream = 0;

        private BPMProcedure bpmProc;
        private void MyBPMProc(int chan, float BPM, IntPtr user)
        {
            RunOnUiThread(() =>
            {
                // this code runs on the UI thread!
                txtvwBPM.Text = string.Format("BPM = {0:0.00} ", (int)BPM);
                progbarTime.Progress += 10;
                txtvwTime.Text = string.Format("Time: {0}s/{1:0.00}s", progbarTime.Progress, time);
                if (serialIoManager != null) { 
                    byte[] sendBytes = Encoding.ASCII.GetBytes(((int)BPM).ToString() + "\n");
                    
                    port.Write(sendBytes, 10000);
                    
                }
            });
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.mediaPlayer);

            if (!Bass.Init())
            {
                Toast.MakeText(this, "Bass_Init error!", ToastLength.Long).Show();
            }

            usbManager = GetSystemService(UsbService) as UsbManager;

            Button btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnPlay.Click += BtnPlay_Click;

            Button btnStop = FindViewById<Button>(Resource.Id.btnStop);
            btnStop.Click += BtnStop_Click;

            var skbrVolume = FindViewById<SeekBar>(Resource.Id.skbrVolume);
            skbrVolume.Max = 100;
            skbrVolume.Progress = 100;
            skbrVolume.ProgressChanged += SkbrVolume_ProgressChanged;

            txtvwBPM = FindViewById<TextView>(Resource.Id.txtvwBPM);
            txtvwVol = FindViewById<TextView>(Resource.Id.txtvwVol);
            txtvwSongName = FindViewById<TextView>(Resource.Id.txtvwSongName);
            txtvwTime = FindViewById<TextView>(Resource.Id.txtvwTime);
            txtvwError = FindViewById<TextView>(Resource.Id.txtvwError);
            txtvwPorts = FindViewById<TextView>(Resource.Id.txtvwPorts);
            progbarTime = FindViewById<ProgressBar>(Resource.Id.progbarTime);

            progbarTime.Progress = 0;

            var file = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).AbsolutePath + "/Who Did That To You- (John Legend).mp3";
            txtvwSongName.Text = file.Split('/').Last();

            stream = Bass.CreateStream(file, 0, 0, BassFlags.Prescan);

            if (stream != 0)
            {
                time = Bass.ChannelBytes2Seconds(stream, Bass.ChannelGetLength(stream, PositionFlags.Bytes));
                txtvwTime.Text = string.Format("Time: {0}s/{1:0.00}s", 0, time);
                progbarTime.Max = (int)time;
                BassFx.BPMCallbackReset(stream);
                bpmProc = new BPMProcedure(MyBPMProc);
                var b = BassFx.BPMCallbackSet(stream, bpmProc, bpmTimePeriod, BitHelper.MakeLong(45, 240), BassFlags.FXBpmMult2);
                if (!b)
                {
                    txtvwError.Text = string.Format("error: {0}", Bass.LastError);
                    txtvwError.Visibility = ViewStates.Visible;
                }
            }

        }

        private void SkbrVolume_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Bass.Volume = e.Progress / 100f;
            txtvwVol.Text = string.Format("Volume: {0}%", e.Progress);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
           Bass.ChannelStop(stream);
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
           Bass.ChannelPlay(stream, false);
        }


        #region "Overrided base functions"

        protected async override void OnResume()
        {

            base.OnResume();

            var portInfo = Intent.GetParcelableExtra(EXTRA_TAG) as UsbSerialPortInfo ?? null;
            txtvwPorts.Text = (portInfo == null ? "null" : portInfo.ToString());
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
                serialIoManager = new SerialInputOutputManager(port)
                {
                    BaudRate = 9600,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Parity = Parity.None,
                };
                serialIoManager.ErrorReceived += (sender, e) => {
                    RunOnUiThread(() => {
                        var intent = new Intent(this, typeof(MainMenu));
                        StartActivity(intent);
                    });
                };
                serialIoManager.ErrorReceived += (sender, e) => {
                    RunOnUiThread(() => {
                        var intent = new Intent(this, typeof(MainMenu));
                        StartActivity(intent);
                    });
                };
                try
                {
                    serialIoManager.Open(usbManager);
                }
                catch (Java.IO.IOException e)
                {
                    txtvwError.Text = "Error opening device: " + e.Message;
                    txtvwError.Visibility = ViewStates.Visible;
                    return;
                }
            }
        }
           

        protected override void OnDestroy()
        {
            if (serialIoManager != null)
                serialIoManager.Close();
            // Free the stream
            Bass.StreamFree(stream);

            // Free current device.
            Bass.Free();
            base.OnDestroy();
        }

        #endregion
    } 


    
}

