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
    class DataTwoItems
    {
        public int id;

        public String item0;
        public String item1;
        public String item2;

        public String item3;
        public String item4;
        public String item5;
        public String item6;

        public DataTwoItems(int i, String i1, String i2)
        {
            id = i;
            item1 = i1;
            item2 = i2;
        }

        public DataTwoItems(int i, String i0, String i1, String i2)
        {
            id = i;
            item0 = i0;
            item1 = i1;
            item2 = i2;
        }
    }
}