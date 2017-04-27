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
using System.Collections.Generic;
using System.Timers;

namespace haptic_strappack_app
{
    [Activity(Label = "Media Player")]
    public class MediaPlayer : Activity
    {
        public const string PortTag = "PortInfo";        
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
        private Timer timer;

        public const string FileTag = "FileUri";
        private int count;
        List<Android.Net.Uri> fileUri;

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

            Button btnNext = FindViewById<Button>(Resource.Id.btnNextSong);
            btnNext.Click += BtnStop_Click;
            btnNext.Click += BtnNext_Click;

            Button btnPrev = FindViewById<Button>(Resource.Id.btnPrevSong);
            btnPrev.Click += BtnStop_Click;
            btnPrev.Click += BtnPrev_Click; 

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

            count = 0;
            fileUri = new List<Android.Net.Uri>();

            timer = new Timer() { Interval = 1000 };
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                // this code runs on the UI thread!
                progbarTime.Progress += 1;
                txtvwTime.Text = string.Format("Time: {0}s/{1:0}s", progbarTime.Progress, time);
                if (progbarTime.Progress == progbarTime.Max)
                {
                    timer.Stop();
                    BtnNext_Click(sender, e);
                    BtnPlay_Click(sender, e);
                }
            });
           
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            count = (count > 0 ? count - 1 : fileUri.Count-1);
            GetNextSong();
            BtnPlay_Click(sender, e);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            count = (count < fileUri.Count ? count + 1 : 0);            
            GetNextSong();
            BtnPlay_Click(sender, e);
        }

        private void SkbrVolume_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Bass.Volume = e.Progress / 100f;
            txtvwVol.Text = string.Format("Volume: {0}%", e.Progress);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            Bass.ChannelStop(stream);
            
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            
            if (progbarTime.Progress == progbarTime.Max) progbarTime.Progress = 0;
            Bass.ChannelPlay(stream, false);
            timer.Start();

        }

        private void GetNextSong()
        {
            System.IO.Stream fileStream = ContentResolver.OpenInputStream(fileUri[count]);
            var tempFile = OpenFileOutput("TempFile", FileCreationMode.Private);

            fileStream.CopyTo(tempFile);
            fileStream.Close();
            tempFile.Close();

            var file = (this.FilesDir.AbsolutePath + "/TempFile");
            txtvwSongName.Text = fileUri[count].Path.Split('/').Last();

            stream = Bass.CreateStream(file, 0, 0, BassFlags.Prescan);

            if (stream != 0)
            {
                time = Bass.ChannelBytes2Seconds(stream, Bass.ChannelGetLength(stream, PositionFlags.Bytes));
                txtvwTime.Text = string.Format("Time: {0}s/{1:0}s", 0, time);
                progbarTime.Max = (int)time;
                progbarTime.Progress = 0;
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

        #region "Overrided base functions"

        protected async override void OnResume()
        {

            base.OnResume();

            Bundle b = Intent.GetParcelableExtra(FileTag) as Bundle ?? null;
            //List<IParcelable> p = new List<IParcelable>();
            foreach (Android.Net.Uri file in b.GetParcelableArrayList("files"))
            {
                fileUri.Add(file);
            }

            GetNextSong();

            var portInfo = Intent.GetParcelableExtra(PortTag) as UsbSerialPortInfo ?? null;
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

