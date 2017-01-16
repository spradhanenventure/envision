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
using Android.Graphics;

namespace ENVision
{
    class LVTextButtonAdapter : BaseAdapter
    {
        public List<DataTwoItems> myItems = new List<DataTwoItems>();
        public List<Boolean> appliedEvent;
        private Activity _activity;
        private Intent _intent;

        public LVTextButtonAdapter(Activity activity, List<DataTwoItems> itemList, Intent intent)
        {
            _activity = activity;
            myItems = itemList;
            _intent = intent;
        }

        public override int Count
        {
            get
            {
                return myItems.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;

            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = _activity.LayoutInflater.Inflate(Resource.Layout.LVTextButtonLayout, parent, false);
                holder.imageView = convertView.FindViewById<ImageView>(Resource.Id.et0);
                holder.textView = convertView.FindViewById<TextView>(Resource.Id.et1);
                holder.buttonView = convertView.FindViewById<Button>(Resource.Id.et2);

                convertView.Tag = holder;
            }
            else
                holder = convertView.Tag as ViewHolder;

            holder.reference = position;

            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.LVTextEditLayout, parent, false);

            DataTwoItems item = myItems.ElementAt(position);

            holder.buttonView.Text = item.item2;

            if (item.item0.Length > 0)
            {
                Java.IO.File file = new Java.IO.File(item.item0);

                Bitmap bitmap = file.Path.LoadAndResizeBitmap(100, 100);

                holder.textView.Visibility = ViewStates.Gone;
                holder.imageView.SetImageBitmap(bitmap);
            }
            else
            {
                holder.imageView.Visibility = ViewStates.Gone;
                holder.textView.Text = item.item1;
            }

            holder.buttonView.Click += delegate
            {
                _intent.PutExtra("id", myItems[holder.reference].id);
                _intent.PutExtra("name", myItems[holder.reference].item1);
                _intent.PutExtra("item", float.Parse(myItems[holder.reference].item2));
                _activity.StartActivity(_intent);
            };
            
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imageView;
            public TextView textView;
            public Button buttonView;
            public int reference;
        }
    }
}