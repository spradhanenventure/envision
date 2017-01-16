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
    [Activity(Label = "CompleteActivity")]
    public class CompleteActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CompleteActivity);

            // Create your application here
            Button b_next = FindViewById<Button>(Resource.Id.btnMove);

            b_next.Click += delegate
            {
                Intent intent = new Intent(this, typeof(POSActivity));
                StartActivity(intent);
            };
        }
    }
}