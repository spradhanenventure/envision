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
    class LVPOSAdapter : BaseAdapter, IDialogInterfaceOnClickListener
    {
        public List<DataTwoItems> myItems = new List<DataTwoItems>();
        private Activity _activity;
        ViewHolder activeHolder;
        String date;

        public LVPOSAdapter(Activity activity, List<DataTwoItems> itemList, String d)
        {
            _activity = activity;
            myItems = itemList;
            date = d;
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
                convertView = _activity.LayoutInflater.Inflate(Resource.Layout.LVPOSLayout, parent, false);
                holder.imageView = convertView.FindViewById<ImageView>(Resource.Id.et0);
                holder.text1View = convertView.FindViewById<TextView>(Resource.Id.et1);
                holder.text2View = convertView.FindViewById<TextView>(Resource.Id.et2);

                convertView.Tag = holder;
            }
            else
                holder = convertView.Tag as ViewHolder;

            holder.reference = position;

            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.LVTextEditLayout, parent, false);

            DataTwoItems item = myItems.ElementAt(position);
            
            holder.text2View.Text = item.item2;

            if (item.item0.Length > 0)
            {
                Java.IO.File file = new Java.IO.File(item.item0);

                Bitmap bitmap = file.Path.LoadAndResizeBitmap(100, 100);

                holder.text1View.Visibility = ViewStates.Gone;
                holder.imageView.SetImageBitmap(bitmap);
            }
            else
            {
                holder.imageView.Visibility = ViewStates.Gone;
                holder.text1View.Text = item.item1;
            }

            holder.text2View.Click += delegate
            {
                Intent intent = new Intent(_activity, typeof(SelectStockPOSActivity));
                intent.PutExtra("id", myItems[holder.reference].id);
                intent.PutExtra("qty", int.Parse(myItems[holder.reference].item2));
                intent.PutExtra("max", int.Parse(myItems[holder.reference].item5));
                intent.PutExtra("date", date);
                _activity.StartActivity(intent);
                /*
                int total = int.Parse(myItems[holder.reference].item5) + 1;
                String[] s = new String[total];

                for (int i = 0; i < total; i++)
                    s[i] = i + "";

                activeHolder = holder;

                AlertDialog.Builder builder = new AlertDialog.Builder(_activity);
                builder.SetTitle("Select Quantity");
                builder.SetItems(s, this);

                builder.Show();
                */
            };

            return convertView;
        }
        
        public void OnClick(IDialogInterface dialog, int which)
        {
            activeHolder.text2View.Text = which + "";
            myItems[activeHolder.reference].item2 = which + "";
        }

        private class ViewHolder : Java.Lang.Object
        {
            public ImageView imageView;
            public TextView text1View;
            public TextView text2View;
            public int reference;
        }
    }
}