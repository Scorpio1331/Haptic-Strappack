using Android.App;
using Android.Widget;
using Android.OS;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using System;

namespace Haptic_Strappack_App
{
    [Activity(Label = "Haptic_Strappack_App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            BassNet.Registration("butterworthld@gmail.com", "2X118923152222");
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                // all ok
                // load BASS_FX
                BassFx.BASS_FX_GetVersion();
            }
            else
            {
                Toast.MakeText(this, "Bass_Init error!", ToastLength.Long).Show();
            }
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

           

            var btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;

        }

        private void Btn_Click(object sender, EventArgs e)
        {
            
        }
    }
}

