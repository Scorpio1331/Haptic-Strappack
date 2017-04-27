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
using System.IO;
using Android.Database;
using Android.Provider;
using Java.IO;
using Android.Net;

namespace haptic_strappack_app
{
    [Activity(Label = "Haptic Strappack App Menu", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainMenu : Activity
    {
        
        UsbManager usbManager;
        UsbSerialPort port;

        TextView txtvwSelectedDevice;
        Button btnSelectDevice;

        TextView txtvwFolderSelected;
        Button btnSelectLibrary;
        FolderSelectionFragment folderSelectionFragment;
        LinearLayout lnrlytLibrarySelection;
        LinearLayout lnrlytMediaPlayer;

        TextView txtvwSongSelected;
        Button btnSelectSong;
        LinearLayout lnrlytSongSelection;

        Button btnMediaPlayer;

        private List<Android.Net.Uri> fileUri;
        public List<Android.Net.Uri> FileUri { get => fileUri; set => fileUri = value; }
        


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.mainMenu);

            txtvwSelectedDevice = FindViewById<TextView>(Resource.Id.txtvwSelectedDevice);
            btnSelectDevice = FindViewById<Button>(Resource.Id.btnSelectDevice);
            btnSelectDevice.Click += BtnSelectDevice_Click;

            txtvwFolderSelected = FindViewById<TextView>(Resource.Id.txtvwFolderSelected);
            txtvwFolderSelected.Visibility = ViewStates.Visible;
            btnSelectLibrary = FindViewById<Button>(Resource.Id.btnSelectLibrary);
            btnSelectLibrary.Click += BtnSelectLibrary_Click;
            btnSelectLibrary.Visibility = ViewStates.Visible;

            lnrlytLibrarySelection = FindViewById<LinearLayout>(Resource.Id.lnrlytLibrarySelection);
           

            lnrlytSongSelection = FindViewById<LinearLayout>(Resource.Id.lnrlytSongSelection);
            txtvwSongSelected = FindViewById<TextView>(Resource.Id.txtvwSongSelected);
            btnSelectSong = FindViewById<Button>(Resource.Id.btnSelectSong);    

            lnrlytMediaPlayer = FindViewById<LinearLayout>(Resource.Id.lnrlytMediaPlayer);
            btnMediaPlayer = FindViewById<Button>(Resource.Id.btnMediaPlayer);
            btnMediaPlayer.Click += BtnMediaPlayer_Click;

            folderSelectionFragment = new FolderSelectionFragment();

            fileUri = new List<Android.Net.Uri>();

        }

        private void BtnSelectLibrary_Click(object sender, EventArgs e)
        {
            //var intent = new Intent(Intent.ActionGetContent);
            //intent.SetType("audio/*");
            //intent.AddCategory(Intent.CategoryOpenable);
            //if (intent.ResolveActivity(PackageManager) != null)
            //{
            //    this.StartActivityForResult(Intent.CreateChooser(intent, "Select song"), 1);
            //}
            //else
            //{
            //    Toast.MakeText(this, "No file explorer app installed, defaulting to inbuilt one ", ToastLength.Long).Show();
            var trans = FragmentManager.BeginTransaction();
            trans.SetTransition(FragmentTransit.FragmentFade);
            trans.Add(Resource.Id.frmlytContainer, folderSelectionFragment);
            trans.Commit();
            lnrlytMediaPlayer.Visibility = ViewStates.Gone;
            lnrlytLibrarySelection.Visibility = ViewStates.Gone;
            // }
        }

        public void fileChosen(string filePath)
        {
            var trans = FragmentManager.BeginTransaction();
            trans.SetTransition(FragmentTransit.ExitMask);
            trans.Remove(folderSelectionFragment);
            trans.Commit();
            lnrlytLibrarySelection.Visibility = ViewStates.Visible;
            lnrlytMediaPlayer.Visibility = ViewStates.Visible;
            txtvwFolderSelected.Visibility = ViewStates.Visible;
            txtvwFolderSelected.Text = (FileUri.Count > 1 ? "Folder: " : "File: ") + filePath;
            btnSelectLibrary.Visibility = ViewStates.Visible;
            btnMediaPlayer.Enabled = true;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && requestCode == 1)
            {
                FileUri.Add(data.Data);
            }
           
        }
        protected override async void OnResume()
        {
            base.OnResume();

            //if (fileUri.Count == 0)
            //    folderSelectionFragment.RefreshFilesList(Android.OS.Environment.RootDirectory.AbsolutePath);

            usbManager = GetSystemService(UsbService) as UsbManager;

            var portInfo = (UsbSerialPortInfo)Intent.GetParcelableExtra(MediaPlayer.PortTag) ?? null;
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
            
            if (port != null ) newIntent.PutExtra(MediaPlayer.PortTag, new UsbSerialPortInfo(port));
            Bundle b = new Bundle();
            List<IParcelable> p = new List<IParcelable>();
            foreach (var file in FileUri)
            {
                p.Add(file);
            }
            b.PutParcelableArrayList("files", p);
            newIntent.PutExtra(MediaPlayer.FileTag,b);
            StartActivity(newIntent);
        }

        private void BtnSelectDevice_Click(object sender, EventArgs e)
        {
            var newIntent = new Intent(this, typeof(DeviceSelection));
            StartActivity(newIntent);

        }
    }
}