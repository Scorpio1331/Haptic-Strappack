using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.IO;

namespace haptic_strappack_app
{
    public class FolderSelectionFragment : ListFragment
    {
        public static readonly string DefaultInitialDirectory = Android.OS.Environment.RootDirectory.AbsolutePath;
        private FileListAdapter _adapter;
        private DirectoryInfo _directory;
        private string[] extensionList = { ".mp3",".mp2",".mp1",".ogg",".wav",".aiff" };

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _adapter = new FileListAdapter(Activity, new FileSystemInfo[0]);
            ListAdapter = _adapter;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            FileSystemInfo fileSystemInfo = _adapter.GetItem(position);
            MainMenu menu = Context as MainMenu;
            
            
            if (!fileSystemInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
               
                    menu.FileUri.Add( Android.Net.Uri.FromFile(new Java.IO.File(fileSystemInfo.FullName)));
                    menu.fileChosen(fileSystemInfo.FullName);
                
            }
            else
            {

                if (fileSystemInfo.Name == "Select this folder")
                {
                    string[] parentPath = _adapter.GetItem(position - 1).FullName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string parent = "";
                    for (int i = 0; i < parentPath.Length - 1; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(parentPath[i]))
                            parent += parentPath[i] + "/";
                    }

                    var fileList =   new DirectoryInfo(parent).EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);

                    foreach (var file in fileList)
                    {
                        if (extensionList.Contains(file.Extension))
                        {
                            menu.FileUri.Add(Android.Net.Uri.FromFile(new Java.IO.File(file.FullName)));
                        }
                    }
                    menu.fileChosen(parent);
                }
                else if (fileSystemInfo.Name == "/")
                {
                    string[] parentPath = _adapter.GetItem(position + 1).FullName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string parent = "";
                    for (int i = 0; i < parentPath.Length-2; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(parentPath[i]))
                            parent += parentPath[i] + "/";
                    }
                    parent = parent.Substring(0,parent.Length - 1);
                    RefreshFilesList(parent);
                }
                else
                {
                    // Dig into this directory, and display it's contents
                    RefreshFilesList(fileSystemInfo.FullName);
                }
                
            }

            base.OnListItemClick(l, v, position, id);
        }

        public override void OnResume()
        {
            base.OnResume();
            RefreshFilesList(DefaultInitialDirectory);
        }

        public void RefreshFilesList(string directory)
        {
            IList<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
            var dir = new DirectoryInfo(directory);

            try
            {
                if (dir.FullName != DefaultInitialDirectory) visibleThings.Add(new DirectoryInfo(".."));
                foreach (var item in dir.GetFileSystemInfos().Where(item => !item.Attributes.HasFlag(FileAttributes.Hidden)))
                {
                    visibleThings.Add(item);
                }
                visibleThings.Add(new DirectoryInfo("Select this folder"));
            }
            catch (Exception ex)
            {
                Toast.MakeText(this.Context, "Problem retrieving contents of " + directory + " because " + ex.Message, ToastLength.Long).Show();

                return;
            }

            this._directory = dir;

            _adapter.AddDirectoryContents(visibleThings);

            // If we don't do this, then the ListView will not update itself when then data set 
            // in the adapter changes. It will appear to the user that nothing has happened.
            ListView.RefreshDrawableState();
        }

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    // Use this to return your custom view for this Fragment
        //    return inflater.Inflate(Resource.Layout.folderSelection, container,false);

        //    //return base.OnCreateView(inflater, container, savedInstanceState);
        //}
    }
}