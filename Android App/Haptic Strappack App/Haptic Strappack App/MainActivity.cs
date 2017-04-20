using Android.App;
using Android.Widget;
using Android.OS;
using System.Linq;
using System;
using ManagedBass;
using ManagedBass.Fx;

namespace Haptic_Strappack_App
{
    [Activity(Label = "Haptic_Strappack_App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView txtvwBPM;
        private TextView txtvwVol;
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
                txtvwBPM.Text = string.Format("BPM = {0:0.00} ", BPM);
                progbarTime.Progress += 10;
                txtvwTime.Text = string.Format("Time: {0}s/{1:0.00}s", progbarTime.Progress, time);
            });
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (!Bass.Init())
            {
                Toast.MakeText(this, "Bass_Init error!", ToastLength.Long).Show();
            }

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            var btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnPlay.Click += BtnPlay_Click;

            var btnStop = FindViewById<Button>(Resource.Id.btnStop);
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
            progbarTime = FindViewById<ProgressBar>(Resource.Id.progbarTime);

            progbarTime.Progress = 0;

            var file = "/sdcard/e-dubble+-+Let+Me+Oh+(Freestyle+Friday+#9).mp3";
            txtvwSongName.Text = file.Split('/').Last();

            stream = Bass.CreateStream(file,0,0,BassFlags.Prescan);

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
                    txtvwError.Visibility = Android.Views.ViewStates.Visible;
                }
                Bass.ChannelPlay(stream);
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

        


        protected override void OnDestroy()
        {
            // Free the stream
            Bass.StreamFree(stream);

            // Free current device.
            Bass.Free();
            base.OnDestroy();
        }
    }
}

