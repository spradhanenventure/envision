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
    [Activity(Label = "Saving Product")]
    public class LoadingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.LoadingActivity);

            Handler handler = new Handler();
            handler.PostDelayed(() =>
            {
                Intent intent = new Intent(this, typeof(AddCostActivity));
                StartActivity(intent);
            }, 3000);
            /*
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {

            });
            */
        }
    }
}