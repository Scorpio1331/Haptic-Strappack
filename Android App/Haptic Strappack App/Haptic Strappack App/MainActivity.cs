using Android.App;
using Android.Widget;
using Android.OS;
//using Un4seen.Bass;
//using Un4seen.Bass.AddOn.Fx;
using System;
using ManagedBass;
using ManagedBass.Fx;

namespace Haptic_Strappack_App
{
    [Activity(Label = "Haptic_Strappack_App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextView txtvwBPM;
        private TextView txtvwProg;
        private TextView txtvwError;
        private ProgressBar progbarTime;
        private int stream = 0;

        private BPMProcedure bpmProc;
        private void MyBPMProc(int chan, float BPM, IntPtr user)
        {
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    // this code runs on the UI thread!
                    this.txtvwBPM.Text = string.Format("{0:0.00}, ", BPM);
                    progbarTime.Progress += 10;
                });
            
           
        }

        private BPMProgressProcedure bpmProg;
        private void MyBPMProg(int chan, float per, IntPtr user)
        {
            RunOnUiThread(delegate
            {
                // this code runs on the UI thread!
                this.txtvwProg.Text = string.Format("{0}%, ", per);
                progbarTime.Progress = (int)per;
            });
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //BassNet.Registration("butterworthld@gmail.com", "2X118923152222");
            //if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            //{
            //    // all ok
            //    // load BASS_FX
            //    BassFx.BASS_FX_GetVersion();
            //}
            //else
            //{
            //    Toast.MakeText(this, "Bass_Init error!", ToastLength.Long).Show();
            //}
            if (!Bass.Init())
            {
                Toast.MakeText(this, "Bass_Init error!", ToastLength.Long).Show();
            }

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

           

            var btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnPlay.Click += BtnPlay_Click;

            var btnStop = FindViewById<Button>(Resource.Id.btnStop);
            btnStop.Click += BtnStop_Click;

            txtvwBPM = FindViewById<TextView>(Resource.Id.txtvwBPM);
            txtvwProg = FindViewById<TextView>(Resource.Id.txtvwProg);
            txtvwError = FindViewById<TextView>(Resource.Id.txtvwError);
            progbarTime = FindViewById<ProgressBar>(Resource.Id.progbarTime);

            progbarTime.Progress = 0;

            var file = "/sdcard/e-dubble+-+Let+Me+Oh+(Freestyle+Friday+#9).mp3";

            stream = Bass.CreateStream(file,0,0,BassFlags.Prescan | BassFlags.Decode);

            if (stream != 0)
            {
                double max = Bass.ChannelBytes2Seconds(stream, Bass.ChannelGetLength(stream, PositionFlags.Bytes));
                progbarTime.Max = (int)max;
                BassFx.BPMCallbackReset(stream);
                bpmProc = new BPMProcedure(MyBPMProc);
                var b = BassFx.BPMCallbackSet(stream, bpmProc,10f, BitHelper.MakeLong(45, 240), BassFlags.FXBpmMult2);
                if (!b)
                {
                    txtvwError.Text = string.Format("error: {0}", Bass.LastError);
                    txtvwError.Visibility = Android.Views.ViewStates.Visible;
                }
                Bass.ChannelPlay(stream);
                Bass.Volume = 0.8;
            }
            

            


            //stream = Bass.BASS_StreamCreateFile(file, 0, 0,
            //    BASSFlag.BASS_STREAM_DECODE);

            //bpmProg = new BPMPROGRESSPROC(MyBPMProg);
            //label2.Text = String.Format("BPM={0}", BassFx.BASS_FX_BPM_DecodeGet(stream, 0f, time, Utils.MakeLong(45, 240), BASSFXBpm.BASS_FX_BPM_BKGRND | BASSFXBpm.BASS_FX_FREESOURCE | BASSFXBpm.BASS_FX_BPM_MULT2, bpmProg, IntPtr.Zero));

            //BassFx.BASS_FX_BPM_Free(stream);

            //stream = Bass.BASS_StreamCreateFile(file, 0, 0,
            //   BASSFlag.BASS_DEFAULT);

            //long len = Bass.BASS_ChannelGetLength(stream);
            //progbarTime.Max = (int)len;
            //bpmProc = new BPMPROC(MyBPMProc);
            //txtvwBPM.Text = String.Format("BPM={0}", BassFx.BASS_FX_BPM_CallbackSet(stream, bpmProc, 10f, Utils.MakeLong(45, 240), BASSFXBpm.BASS_FX_BPM_MULT2, IntPtr.Zero));
            //txtvwBPM.Text = string.Format("error: {0}", Bass.BASS_ErrorGetCode());


            //Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 1);
            //Bass.BASS_ChannelPlay(stream, false);
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

