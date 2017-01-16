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
    class LVTextEditAdapter : BaseAdapter
    {
        //private LayoutInflater mInflator;
        public List<DataTwoItems> myItems = new List<DataTwoItems>();
        public List<Boolean> appliedEvent;
        private Activity _activity;

        //public LVTextEditAdapter(Context context, List<DataTwoItems> itemList)
        public LVTextEditAdapter(Activity activity, List<DataTwoItems> itemList)
        {
            _activity = activity;
            //mInflator = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            myItems = itemList;

            //NotifyDataSetChanged();

            //appliedEvent = new List<Boolean>();
           // for (int i = 0; i < myItems.Count; i++)
               // appliedEvent.Add(false);
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
                convertView = _activity.LayoutInflater.Inflate(Resource.Layout.LVTextEditLayout, parent, false);
                holder.imageView = convertView.FindViewById<ImageView>(Resource.Id.et0);
                holder.textView = convertView.FindViewById<TextView>(Resource.Id.et1);
                holder.editView = convertView.FindViewById<EditText>(Resource.Id.et2);

                convertView.Tag = holder;
            }
            else
                holder = convertView.Tag as ViewHolder;

            holder.reference = position;

            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.LVTextEditLayout, parent, false);
            
            //var et1 = view.FindViewById<TextView>(Resource.Id.et1);
            //var et2 = view.FindViewById<TextView>(Resource.Id.et2);
            
            DataTwoItems item = myItems.ElementAt(position);

            //et1.Text = item.item1;
            //et2.Text = item.item2;
            
            if(item.item0.Length > 0)
                holder.imageView.SetImageBitmap(BitmapFactory.DecodeFile(item.item0));

            holder.textView.Text = item.item1;
            holder.editView.Text = item.item2;

            //et2.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
            holder.editView.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
                    myItems[holder.reference].item2 = (sender as EditText).Text;
            };

            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imageView;
            public TextView textView;
            public EditText editView;
            public int reference;
        }
    }
}