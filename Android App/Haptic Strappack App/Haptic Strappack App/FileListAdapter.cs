using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace haptic_strappack_app
{
    class FileListAdapter : ArrayAdapter<FileSystemInfo>
    {
        private readonly Context context;

        public FileListAdapter(Context context, IList<FileSystemInfo> fsi)
            : base(context, Resource.Layout.folderSelection, Android.Resource.Id.Text1, fsi)
        {
            this.context = context;
        }

        //public override FileSystemInfo this[int position]
        //{
        //    get { return fileList[position];  }
        //}
        

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var fileSystemEntry = GetItem(position);

            FolderListViewHolder viewHolder;
            View row;
            if (convertView == null)
            {
                row = LayoutInflater.FromContext(context).Inflate(Resource.Layout.folderSelection, parent, false);
                viewHolder = new FolderListViewHolder(row.FindViewById<TextView>(Resource.Id.txtvwFileName), row.FindViewById<ImageView>(Resource.Id.imgFileImage));
                row.Tag = viewHolder;
            }
            else
            {
                row = convertView;
                viewHolder = (FolderListViewHolder)row.Tag;
            }
            viewHolder.Update(fileSystemEntry.Name, fileSystemEntry.Attributes.HasFlag(FileAttributes.Directory) ? Resource.Drawable.folder : Resource.Drawable.file);

            return row;
        }

        public void AddDirectoryContents(IEnumerable<FileSystemInfo> directoryContents)
        {
            Clear();
            if (directoryContents.Any())
            {
                
                AddAll(directoryContents.ToArray());

                NotifyDataSetChanged();
            }
            else
            {
                NotifyDataSetInvalidated();
            }
        }

    }
}