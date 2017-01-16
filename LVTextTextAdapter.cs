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

namespace ENVision
{
    class LVTextTextAdapter : BaseAdapter
    {
        public List<DataTwoItems> myItems = new List<DataTwoItems>();
        public List<Boolean> appliedEvent;
        private Activity _activity;

        public LVTextTextAdapter(Activity activity, List<DataTwoItems> itemList)
        {
            _activity = activity;
            myItems = itemList;
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
                convertView = _activity.LayoutInflater.Inflate(Resource.Layout.LVTextTextLayout, parent, false);
                holder.text1View = convertView.FindViewById<TextView>(Resource.Id.et1);
                holder.text2View = convertView.FindViewById<TextView>(Resource.Id.et2);

                convertView.Tag = holder;
            }
            else
                holder = convertView.Tag as ViewHolder;

            holder.reference = position;

            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.LVTextEditLayout, parent, false);
            
            DataTwoItems item = myItems.ElementAt(position);
            
            holder.text1View.Text = item.item1;
            holder.text2View.Text = item.item2;

            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView text1View;
            public TextView text2View;
            public int reference;
        }
    }
}